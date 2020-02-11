using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class FloatConstraint : VariableConstraint
	{
		public const float DefaultMinimum = 0.0f;
		public const float DefaultMaximum = 10.0f;

		public override VariableType Type => VariableType.Float;

		public bool HasMinimum;
		public bool HasMaximum;
		public float Minimum = DefaultMinimum;
		public float Maximum = DefaultMaximum;

		public FloatConstraint()
		{
		}

		public FloatConstraint(float? minimum, float? maximum)
		{
			HasMinimum = minimum.HasValue;
			Minimum = minimum ?? DefaultMinimum;
			HasMaximum = maximum.HasValue;
			Maximum = maximum ?? DefaultMaximum;
		}

		public override string ToString()
		{
			var minimum = HasMinimum ? Minimum.ToString() : string.Empty;
			var maximum = HasMaximum ? Maximum.ToString() : string.Empty;

			return string.Format($"{minimum},{maximum}");
		}

		public override Variable Generate()
		{
			var minimum = HasMinimum ? Minimum : float.MinValue;
			var maximum = HasMaximum ? Maximum : float.MaxValue;
			var value = Mathf.Clamp(0.0f, Minimum, Maximum);

			return Variable.Float(value);
		}

		public override bool IsValid(Variable variable)
		{
			if (variable.TryGetFloat(out var value))
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
