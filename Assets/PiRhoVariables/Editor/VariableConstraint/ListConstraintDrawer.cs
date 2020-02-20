using PiRhoSoft.Utilities.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	[CustomPropertyDrawer(typeof(ListConstraint))]
	public class ListConstraintField : PropertyDrawer
	{
		public const string Stylesheet = "ListConstraintStyle.uss";
		public const string UssClassName = "pirho-list-constraint";
		public const string TypeConstraintUssClassName = UssClassName + "__type";

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var typeProperty = property.FindPropertyRelative("_itemType");
			var constraintProperty = property.FindPropertyRelative("_itemConstraint");

			var container = new VisualElement();
			var listContainer = new VisualElement();

			var dropdown = new EnumField { bindingPath = typeProperty.propertyPath };
			dropdown.AddToClassList(TypeConstraintUssClassName);
			dropdown.RegisterValueChangedCallback(evt =>
			{
				using (new ChangeScope(property.serializedObject))
				{
					var constraint = VariableConstraint.Create((VariableType)evt.newValue);
					var value = constraint != null
						? constraint.Generate()
						: Variable.Create((VariableType)evt.newValue);


					constraintProperty.managedReferenceValue = constraint;
		
					listContainer.Clear();
					constraintProperty = constraintProperty.serializedObject.FindProperty(constraintProperty.propertyPath);
					listContainer.Add(CreateConstraint(constraintProperty));
				}
			});

			listContainer.Add(CreateConstraint(constraintProperty));

			container.AddToClassList(UssClassName);
			container.Add(dropdown);
			container.Add(listContainer);
			container.AddStyleSheet(Stylesheet);

			return container;
		}

		private VisualElement CreateConstraint(SerializedProperty property)
		{
			if (property.HasManagedReferenceValue())
			{
				var referenceType = property.GetManagedReferenceValueType();
				var drawerType = PropertyDrawerExtensions.GetDrawerTypeForType(referenceType);

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
				return field;
			}
			else
			{
				return new VisualElement();
			}
		}
	}
}
