using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	[CustomPropertyDrawer(typeof(VariableDefinition))]
	public class VariableDefinitionDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			return new VariableDefinitionField(property, true);
		}
	}
}
