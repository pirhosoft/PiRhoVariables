using PiRhoSoft.Utilities.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	public class VariableDefinitionField : BindableElement
	{
		private const string _namePropertyName = nameof(VariableDefinition.Name);
		private const string _typePropertyName = "_type";
		private const string _constraintPropertyName = "_constraint";
		private const string _valuePropertyName = "_defaultValue";

		#region Style Strings

		public const string Stylesheet = "VariableDefinitionStyle.uss";
		public const string UssClassName = "pirho-variable-definition";
		public const string ContainerUssClassName = UssClassName + "__container";
		public const string NameLabelUssClassName = UssClassName + "__name-label";
		public const string NameFieldUssClassName = UssClassName + "__name-field";
		public const string TypeUssClassName = UssClassName + "__type";
		public const string ConstraintUssClassName = UssClassName + "__constraint";

		#endregion

		private readonly VariableDefinition _definition;

		private readonly SerializedProperty _nameProperty;
		private readonly SerializedProperty _typeProperty;
		private SerializedProperty _constraintProperty;
		private SerializedProperty _valueProperty;

		private readonly VisualElement _rootContainer;
		private readonly VisualElement _horizontalContainer;
		private readonly VisualElement _nameField;
		private readonly VisualElement _typeField;
		private readonly SerializedVariableField _valueField;
		private VisualElement _constraintField;

		public VariableDefinitionField(SerializedProperty property, bool allowNameChange)
		{
			_rootContainer = new VisualElement();

			bindingPath = property.propertyPath;
			_definition = property.GetObject<VariableDefinition>();

			_nameProperty = property.FindPropertyRelative(_namePropertyName);
			_typeProperty = property.FindPropertyRelative(_typePropertyName);
			_constraintProperty = property.FindPropertyRelative(_constraintPropertyName);
			_valueProperty = property.FindPropertyRelative(_valuePropertyName);

			_horizontalContainer = new VisualElement();
			_horizontalContainer.AddToClassList(ContainerUssClassName);

			_nameField = CreateName(_nameProperty, allowNameChange);
			_typeField = CreateType(_typeProperty);
			_constraintField = CreateConstraint(_constraintProperty);
			_valueField = CreateValue(_valueProperty, _definition);

			_horizontalContainer.Add(_nameField);
			_horizontalContainer.Add(_typeField);

			_rootContainer.Add(_horizontalContainer);
			_rootContainer.Add(_constraintField);
			_rootContainer.Add(_valueField);

			Add(_rootContainer);
			AddToClassList(UssClassName);
			this.AddStyleSheet(Stylesheet);
		}

		private void TypeChanged(VariableType newType)
		{
			using (new ChangeScope(_constraintProperty.serializedObject))
			{
				var constraint = VariableConstraint.Create(newType);
				var value = constraint != null
					? constraint.Generate()
					: Variable.Create(newType);

				_constraintProperty.managedReferenceValue = constraint;
				_valueField.Inject(value);
			}

			_constraintProperty = _constraintProperty.serializedObject.FindProperty(_constraintProperty.propertyPath);
			_constraintField?.RemoveFromHierarchy();
			_constraintField = CreateConstraint(_constraintProperty);
			_rootContainer.Insert(1, _constraintField);
		}

		private void ConstraintChanged()
		{
			if (!_definition.IsValid(_definition.DefaultValue))
			{
				using (new ChangeScope(_constraintProperty.serializedObject))
				{
					var value = _definition.Generate();
					_valueField.Inject(value);
				}
			}

			_valueField.Control.Refresh();
		}

		private VisualElement CreateName(SerializedProperty property, bool allowNameChange)
		{
			if (allowNameChange)
			{
				var nameField = new TextField();
				nameField.BindProperty(property);
				nameField.AddToClassList(NameFieldUssClassName);

				return nameField;
			}
			else
			{
				var nameLabel = new Label();
				nameLabel.text = property.stringValue;
				nameLabel.AddToClassList(NameLabelUssClassName);

				return nameLabel;
			}
		}

		private VisualElement CreateType(SerializedProperty property)
		{
			var field = new EnumField();
			field.BindProperty(property);
			field.AddToClassList(TypeUssClassName);
			field.RegisterValueChangedCallback(evt => TypeChanged((VariableType)evt.newValue));

			return field;
		}

		private class GenericChangeEvent : EventBase<GenericChangeEvent> { }

		private class ChangeEventTrap : VisualElement
		{
			public VariableDefinitionField Field;

			public override void HandleEvent(EventBase evt)
			{
				if (evt is IChangeEvent)
				{
					Field.ConstraintChanged();
					evt.StopImmediatePropagation();
				}
				else
				{
					base.HandleEvent(evt);
				}
			}
		}

		private VisualElement CreateConstraint(SerializedProperty property)
		{
			if (property.HasManagedReferenceValue())
			{
				var referenceType = property.GetManagedReferenceValueType();
				var drawerType = PropertyDrawerExtensions.GetDrawerTypeForType(referenceType);
				var root = new ChangeEventTrap { Field = this };
				root.RegisterCallback<GenericChangeEvent>(evt => { });

				VisualElement field;

				if (drawerType != null)
				{
					var drawer = drawerType.CreateInstance<PropertyDrawer>();
					field = drawer.CreatePropertyGUI(property);
				}
				else
				{
					field = new InlineField(property, true);
				}

				field.Bind(property.serializedObject);
				root.Add(field);
				return root;
			}
			else
			{
				return new VisualElement();
			}
		}

		private SerializedVariableField CreateValue(SerializedProperty property, VariableDefinition definition)
		{
			var field = new SerializedVariableField(property, definition);
			field.SetFieldLabel(null);
			return field;
		}
	}
}
