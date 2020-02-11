using PiRhoSoft.Utilities;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	internal class Vector2VariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable variable)
		{
			return variable.AsVector2.ToString();
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			var vector = variable.AsVector2;

			writer.Writer.Write(vector.x);
			writer.Writer.Write(vector.y);
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var x = reader.Reader.ReadSingle();
			var y = reader.Reader.ReadSingle();

			return Variable.Vector2(new Vector2(x, y));
		}

		protected internal override Variable Add(Variable left, Variable right)
		{
			if (right.TryGetVector2(out var vector))
				return Variable.Vector2(left.AsVector2 + vector);
			else
				return Variable.Empty;
		}

		protected internal override Variable Subtract(Variable left, Variable right)
		{
			if (right.TryGetVector2(out var vector))
				return Variable.Vector2(left.AsVector2 - vector);
			else
				return Variable.Empty;
		}

		protected internal override Variable Multiply(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var f))
				return Variable.Vector2(left.AsVector2 * f);
			else
				return Variable.Empty;
		}

		protected internal override Variable Divide(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var f))
				return Variable.Vector2(left.AsVector2 / f);
			else
				return Variable.Empty;
		}

		protected internal override Variable Negate(Variable value)
		{
			var vector = value.AsVector2;
			return Variable.Vector2(new Vector2(-vector.x, -vector.y));
		}

		protected internal override Variable Lookup(Variable owner, Variable lookup)
		{
			if (lookup.TryGetString(out var s))
			{
				var vector = owner.AsVector2;

				switch (s)
				{
					case "x": return Variable.Float(vector.x);
					case "y": return Variable.Float(vector.y);
				}
			}

			return Variable.Empty;
		}

		protected internal override SetVariableResult Assign(ref Variable owner, Variable lookup, Variable value)
		{
			if (value.TryGetFloat(out var f))
			{
				if (lookup.TryGetString(out var s))
				{
					var vector = owner.AsVector2;

					switch (s)
					{
						case "x":
						{
							owner = Variable.Vector2(new Vector2(f, vector.y));
							return SetVariableResult.Success;
						}
						case "y":
						{
							owner = Variable.Vector2(new Vector2(vector.x, f));
							return SetVariableResult.Success;
						}
					}
				}

				return SetVariableResult.NotFound;
			}
			else
			{
				return SetVariableResult.TypeMismatch;
			}
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.TryGetVector2(out var vector))
				return left.AsVector2 == vector;
			else
				return null;
		}

		protected internal override float Distance(Variable from, Variable to)
		{
			if (to.TryGetVector2(out var t))
				return Vector2.Distance(from.AsVector2, t);
			else
				return 0.0f;
		}

		protected internal override Variable Interpolate(Variable from, Variable to, float time)
		{
			if (to.TryGetVector2(out var t))
			{
				var lerped = Vector2.Lerp(from.AsVector2Int, t, time);
				return Variable.Vector2(lerped);
			}
			else
			{
				return Variable.Empty;
			}
		}
	}
}
