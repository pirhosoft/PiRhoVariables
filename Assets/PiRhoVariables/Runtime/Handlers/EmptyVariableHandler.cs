using PiRhoSoft.Utilities;

namespace PiRhoSoft.Variables
{
	internal class EmptyVariableHandler : VariableHandler
	{
		public const string EmptyText = "(empty)";

		protected internal override string ToString(Variable variable)
		{
			return EmptyText;
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			return Variable.Empty;
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			return right.IsEmpty || right.IsNullObject;
		}
	}
}
