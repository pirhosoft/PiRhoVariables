using UnityEngine;

namespace PiRhoSoft.Variables
{
	public class GlobalVariablesBehaviour : MonoBehaviour
	{
		public SerializedVariableDictionary Variables = new SerializedVariableDictionary();

		private void OnEnable()
		{
			VariableContext.Default.AddVariables(Variables);
		}

		private void OnDisable()
		{
			VariableContext.Default.RemoveVariables(Variables);
		}
	}
}
