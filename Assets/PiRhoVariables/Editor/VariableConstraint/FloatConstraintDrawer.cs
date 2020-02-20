using PiRhoSoft.Utilities.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	[CustomPropertyDrawer(typeof(FloatConstraint))]
	public class FloatConstraintDrawer : PropertyDrawer
	{
		public const string Stylesheet = "FloatConstraintStyle.uss";
		public const string UssClassName = "pirho-float-constraint";
		public const string ContainerUssClassName = UssClassName + "__container";
		public const string ContainerMinUssClassName = ContainerUssClassName + "--min";
		public const string ContainerMaxUssClassName = ContainerUssClassName + "--max";
		public const string LabelUssClassName = UssClassName + "__label";
		public const string ToggleUssClassName = UssClassName + "__toggle";
		public const string MinToggleUssClassName = ToggleUssClassName + "--min";
		public const string MaxToggleUssClassName = ToggleUssClassName + "--max";
		public const string FieldUssClassName = UssClassName + "__field";
		public const string HasValueUssClassName = FieldUssClassName + "--has-value";
		public const string MinUssClassName = FieldUssClassName + "--min";
		public const string MaxUssClassName = FieldUssClassName + "--max";

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var hasMinProperty = property.FindPropertyRelative(nameof(FloatConstraint.HasMinimum));
			var hasMaxProperty = property.FindPropertyRelative(nameof(FloatConstraint.HasMaximum));
			var minProperty = property.FindPropertyRelative(nameof(FloatConstraint.Minimum));
			var maxProperty = property.FindPropertyRelative(nameof(FloatConstraint.Maximum));

			var container = new VisualElement();
			var minContainer = new VisualElement();
			var maxContainer = new VisualElement();
			var minLabel = new TextElement { text = "Minimum:" };
			var maxLabel = new TextElement { text = "Maximum:" };
			var minToggle = new Toggle { bindingPath = hasMinProperty.propertyPath };
			var maxToggle = new Toggle { bindingPath = hasMaxProperty.propertyPath };
			var min = new FloatField { bindingPath = minProperty.propertyPath };
			var max = new FloatField { bindingPath = maxProperty.propertyPath };

			minToggle.RegisterValueChangedCallback(evt => min.EnableInClassList(HasValueUssClassName, evt.newValue));
			maxToggle.RegisterValueChangedCallback(evt => max.EnableInClassList(HasValueUssClassName, evt.newValue));

			container.AddToClassList(UssClassName);
			minContainer.AddToClassList(ContainerUssClassName);
			minContainer.AddToClassList(ContainerMinUssClassName);
			maxContainer.AddToClassList(ContainerUssClassName);
			maxContainer.AddToClassList(ContainerMaxUssClassName);
			minLabel.AddToClassList(LabelUssClassName);
			maxLabel.AddToClassList(LabelUssClassName);
			minToggle.AddToClassList(ToggleUssClassName);
			minToggle.AddToClassList(MinToggleUssClassName);
			maxToggle.AddToClassList(ToggleUssClassName);
			maxToggle.AddToClassList(MaxToggleUssClassName);
			min.AddToClassList(FieldUssClassName);
			min.AddToClassList(MinUssClassName);
			max.AddToClassList(FieldUssClassName);
			max.AddToClassList(MaxUssClassName);

			minContainer.Add(minLabel);
			minContainer.Add(minToggle);
			minContainer.Add(min);
			maxContainer.Add(maxLabel);
			maxContainer.Add(maxToggle);
			maxContainer.Add(max);

			container.Add(minContainer);
			container.Add(maxContainer);
			container.AddStyleSheet(Stylesheet);

			return container;
		}
	}
}
