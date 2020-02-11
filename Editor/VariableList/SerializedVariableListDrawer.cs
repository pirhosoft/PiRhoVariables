using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	[CustomPropertyDrawer(typeof(SerializedVariableList))]
	public class SerializedVariableListDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			return new SerializedVariableListField(property);
		}
	}
}
