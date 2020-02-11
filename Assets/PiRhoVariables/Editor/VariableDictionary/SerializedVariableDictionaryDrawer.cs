using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	[CustomPropertyDrawer(typeof(SerializedVariableDictionary))]
	public class SerializedVariableDictionaryDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			return new SerializedVariableDictionaryField(property);
		}
	}
}
