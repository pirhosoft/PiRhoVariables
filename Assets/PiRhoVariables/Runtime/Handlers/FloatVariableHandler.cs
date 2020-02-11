using PiRhoSoft.Utilities;
using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	internal class FloatVariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable variable)
		{
			return variable.AsFloat.ToString();
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			writer.Writer.Write(variable.AsFloat);
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var f = reader.Reader.ReadSingle();
			return Variable.Float(f);
		}

		protected internal override Variable Add(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return Variable.Float(left.AsFloat + number);
			else if (right.Type == VariableType.String)
				return Variable.String(left.AsFloat + right.AsString);
			else
				return Variable.Empty;
		}

		protected internal override Variable Subtract(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return Variable.Float(left.AsFloat - number);
			else
				return Variable.Empty;
		}

		protected internal override Variable Multiply(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return Variable.Float(left.AsFloat * number);
			else
				return Variable.Empty;
		}

		protected internal override Variable Divide(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return Variable.Float(left.AsFloat / number);
			else
				return Variable.Empty;
		}

		protected internal override Variable Modulo(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return Variable.Float(left.AsFloat % number);
			else
				return Variable.Empty;
		}

		protected internal override Variable Exponent(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return Variable.Float((float)Math.Pow(left.AsFloat, number));
			else
				return Variable.Empty;
		}

		protected internal override Variable Negate(Variable value)
		{
			return Variable.Float(-value.AsFloat);
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return left.AsFloat == number;
			else
				return null;
		}

		protected internal override int? Compare(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return left.AsFloat.CompareTo(number);
			else
				return null;
		}

		protected internal override float Distance(Variable from, Variable to)
		{
			if (to.TryGetFloat(out var t))
				return from.AsFloat - t;
			else
				return 0.0f;
		}

		protected internal override Variable Interpolate(Variable from, Variable to, float time)
		{
			if (to.TryGetFloat(out var t))
			{
				var value = Mathf.Lerp(from.AsFloat, t, time);
				return Variable.Float(value);
			}
			else
			{
				return Variable.Empty;
			}
		}
	}
}
