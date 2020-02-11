using PiRhoSoft.Utilities.Editor;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	[CustomPropertyDrawer(typeof(SerializedVariable))]
	public class SerializedVariableDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var definition = FindDefinition(property);
			return new SerializedVariableField(property, definition);
		}

		private VariableDefinition FindDefinition(SerializedProperty property)
		{
			var iterations = 3;
			var field = fieldInfo;

			while (field != null && iterations > 0)
			{
				var attribute = field.GetAttribute<VariableConstraintAttribute>();
				if (attribute != null)
					return attribute.GetDefinition(string.Empty);

				property = property.GetParent();
				field = property
					?.GetParentObject<object>()
					?.GetType()
					?.GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

				--iterations;
			}

			return null;
		}
	}
}
