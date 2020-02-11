using PiRhoSoft.Utilities;
namespace PiRhoSoft.Variables
{
	internal class BoolVariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable variable)
		{
			return variable.AsBool.ToString();
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			writer.Writer.Write(variable.AsBool);
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var b = reader.Reader.ReadBoolean();
			return Variable.Bool(b);
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.TryGetBool(out var b))
				return left.AsBool == b;
			else
				return null;
		}

		protected internal override float Distance(Variable from, Variable to)
		{
			if (to.TryGetBool(out var t))
				return 1.0f;
			else
				return 0.0f;
		}

		protected internal override Variable Interpolate(Variable from, Variable to, float time)
		{
			if (to.TryGetBool(out var t))
				return Variable.Bool(time < 0.5f ? from.AsBool : t);
			else
				return Variable.Empty;
		}
	}
}
