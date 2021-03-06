﻿using PiRhoSoft.Utilities;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	internal class Vector2IntVariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable variable)
		{
			return variable.AsVector2Int.ToString();
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			var vector = variable.AsVector2Int;

			writer.Writer.Write(vector.x);
			writer.Writer.Write(vector.y);
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var x = reader.Reader.ReadInt32();
			var y = reader.Reader.ReadInt32();

			return Variable.Vector2Int(new Vector2Int(x, y));
		}

		protected internal override Variable Add(Variable left, Variable right)
		{
			if (right.TryGetVector2Int(out var vector))
				return Variable.Vector2Int(left.AsVector2Int + vector);
			else
				return Variable.Empty;
		}

		protected internal override Variable Subtract(Variable left, Variable right)
		{
			if (right.TryGetVector2Int(out var vector))
				return Variable.Vector2Int(left.AsVector2Int - vector);
			else
				return Variable.Empty;
		}

		protected internal override Variable Multiply(Variable left, Variable right)
		{
			if (right.TryGetInt(out var i))
				return Variable.Vector2Int(left.AsVector2Int * i);
			else
				return Variable.Empty;
		}

		protected internal override Variable Divide(Variable left, Variable right)
		{
			var vector = left.AsVector2Int;

			if (right.TryGetInt(out var i) && i != 0)
				return Variable.Vector2Int(new Vector2Int(vector.x / i, vector.y / i));
			else if (right.TryGetFloat(out var f))
				return Variable.Vector2(new Vector2(vector.x / f, vector.y / f));
			else
				return Variable.Empty;
		}

		protected internal override Variable Negate(Variable value)
		{
			var vector = value.AsVector2Int;
			return Variable.Vector2Int(new Vector2Int(-vector.x, -vector.y));
		}

		protected internal override Variable Lookup(Variable owner, Variable lookup)
		{
			if (lookup.TryGetString(out var s))
			{
				var vector = owner.AsVector2Int;

				switch (s)
				{
					case "x": return Variable.Int(vector.x);
					case "y": return Variable.Int(vector.y);
				}
			}

			return Variable.Empty;
		}

		protected internal override SetVariableResult Assign(ref Variable owner, Variable lookup, Variable value)
		{
			if (value.TryGetInt(out var i))
			{
				if (lookup.TryGetString(out var s))
				{
					var vector = owner.AsVector2Int;

					switch (s)
					{
						case "x":
						{
							owner = Variable.Vector2Int(new Vector2Int(i, vector.y));
							return SetVariableResult.Success;
						}
						case "y":
						{
							owner = Variable.Vector2Int(new Vector2Int(vector.x, i));
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
			if (right.TryGetVector2Int(out var vector))
				return left.AsVector2Int == vector;
			else
				return null;
		}

		protected internal override float Distance(Variable from, Variable to)
		{
			if (to.TryGetVector2Int(out var t))
				return Vector2.Distance(from.AsVector2Int, t);
			else
				return 0.0f;
		}

		protected internal override Variable Interpolate(Variable from, Variable to, float time)
		{
			if (to.TryGetVector2Int(out var t))
			{
				var lerped = Vector2.Lerp(from.AsVector2Int, t, time);
				return Variable.Vector2Int(new Vector2Int((int)lerped.x, (int)lerped.y));
			}
			else
			{
				return Variable.Empty;
			}
		}
	}
}
