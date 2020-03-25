using UnityEngine;

namespace PiRhoSoft.Variables.Samples
{
	public class VariablesSample : MonoBehaviour
	{
		public SerializedVariable Variable = new SerializedVariable();

		[VariableConstraint(0, 10)]
		public SerializedVariable Int = new SerializedVariable();

		[VariableConstraint(0.0f, 0.0f)]
		public SerializedVariable Float = new SerializedVariable();

		[VariableConstraint(typeof(Object))]
		public SerializedVariable Object = new SerializedVariable();

		[VariableConstraint(VariableType.List)]
		public SerializedVariable List = new SerializedVariable();

		public VariableDefinition Definition = new VariableDefinition();
	}
}