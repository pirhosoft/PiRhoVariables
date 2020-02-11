using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class IntConstraint : VariableConstraint
	{
		public const int DefaultMinimum = 0;
		public const int DefaultMaximum = 10;

		public override VariableType Type => VariableType.Int;

		public bool HasMinimum;
		public bool HasMaximum;
		public int Minimum = DefaultMinimum;
		public int Maximum = DefaultMaximum;

		public IntConstraint()
		{
		}

		public IntConstraint(int? minimum, int? maximum)
		{
			Minimum = minimum.HasValue ? minimum.Value : DefaultMinimum;
			Maximum = maximum.HasValue ? maximum.Value : DefaultMaximum;
		}

		public override string ToString()
		{
			var minimum = HasMinimum ? Minimum.ToString() : string.Empty;
			var maximum = HasMaximum ? Maximum.ToString() : string.Empty;

			return string.Format($"{minimum},{maximum}");
		}

		public override Variable Generate()
		{
			var minimum = HasMinimum ? Minimum : int.MinValue;
			var maximum = HasMaximum ? Maximum : int.MaxValue;
			var value = Mathf.Clamp(0, minimum, maximum);

			return Variable.Int(value);
		}

		public override bool IsValid(Variable variable)
		{
			if (variable.TryGetInt(out var value))
			{
				if (HasMinimum && value < Minimum)
					return false;

				if (HasMaximum && value > Maximum)
					return false;

				return true;
			}

			return false;
		}
	}
}
