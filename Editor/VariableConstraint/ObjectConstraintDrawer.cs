using PiRhoSoft.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	[CustomPropertyDrawer(typeof(ObjectConstraint))]
	public class ObjectConstraintField : PropertyDrawer
	{
		public const string Stylesheet = "ObjectConstraintStyle.uss";
		public const string UssClassName = "pirho-object-constraint";
		public const string TypeUssClassName = UssClassName + "__type";

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var typeProperty = property.FindPropertyRelative("_objectType");
			var container = new VisualElement();
			var typeField = new TypePickerField(typeof(Object), true) { bindingPath = typeProperty.propertyPath };
			typeField.AddToClassList(TypeUssClassName);

			container.Add(typeField);
			container.AddToClassList(UssClassName);
			container.AddStyleSheet(Stylesheet);

			return container;
		}
	}
}
