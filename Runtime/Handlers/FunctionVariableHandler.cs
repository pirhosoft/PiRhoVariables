using PiRhoSoft.Utilities;

namespace PiRhoSoft.Variables
{
	internal class FunctionVariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable value) => "Function";
		protected internal override void Save(Variable value, SerializedDataWriter writer) { }
		protected internal override Variable Load(SerializedDataReader reader) => Variable.Empty;
		protected internal override bool? IsEqual(Variable left, Variable right) => left.AsFunction == right.AsFunction;
	}
}
