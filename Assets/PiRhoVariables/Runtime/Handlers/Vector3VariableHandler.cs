﻿using PiRhoSoft.Utilities;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	internal class Vector3VariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable variable)
		{
			return variable.AsVector3.ToString();
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			var vector = variable.AsVector3;

			writer.Writer.Write(vector.x);
			writer.Writer.Write(vector.y);
			writer.Writer.Write(vector.z);
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var x = reader.Reader.ReadSingle();
			var y = reader.Reader.ReadSingle();
			var z = reader.Reader.ReadSingle();

			return Variable.Vector3(new Vector3(x, y, z));
		}

		protected internal override Variable Add(Variable left, Variable right)
		{
			if (right.TryGetVector3(out var vector))
				return Variable.Vector3(left.AsVector3 + vector);
			else
				return Variable.Empty;
		}

		protected internal override Variable Subtract(Variable left, Variable right)
		{
			if (right.TryGetVector3(out var vector))
				return Variable.Vector3(left.AsVector3 - vector);
			else
				return Variable.Empty;
		}

		protected internal override Variable Multiply(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var f))
				return Variable.Vector3(left.AsVector3 * f);
			else
				return Variable.Empty;
		}

		protected internal override Variable Divide(Variable left, Variable right)
		{
			if (right.TryGetFloat(out var number))
				return Variable.Vector3(left.AsVector3 / number);
			else
				return Variable.Empty;
		}

		protected internal override Variable Negate(Variable value)
		{
			var vector = value.AsVector3;

			return Variable.Vector3(new Vector3(-vector.x, -vector.y, -vector.z));
		}

		protected internal override Variable Lookup(Variable owner, Variable lookup)
		{
			if (lookup.TryGetString(out var s))
			{
				var vector = owner.AsVector3;

				switch (s)
				{
					case "x": return Variable.Float(vector.x);
					case "y": return Variable.Float(vector.y);
					case "z": return Variable.Float(vector.z);
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
					var vector = owner.AsVector3;

					switch (s)
					{
						case "x":
						{
							owner = Variable.Vector3(new Vector3(f, vector.y, vector.z));
							return SetVariableResult.Success;
						}
						case "y":
						{
							owner = Variable.Vector3(new Vector3(vector.x, f, vector.z));
							return SetVariableResult.Success;
						}
						case "z":
						{
							owner = Variable.Vector3(new Vector3(vector.x, vector.y, f));
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
			if (right.TryGetVector3(out var vector))
				return left.AsVector3 == vector;
			else
				return null;
		}

		protected internal override float Distance(Variable from, Variable to)
		{
			if (to.TryGetVector3(out var t))
				return Vector3.Distance(from.AsVector3, t);
			else
				return 0.0f;
		}

		protected internal override Variable Interpolate(Variable from, Variable to, float time)
		{
			if (to.TryGetVector3(out var t))
			{
				var lerped = Vector3.Lerp(from.AsVector3, t, time);
				return Variable.Vector3(lerped);
			}
			else
			{
				return Variable.Empty;
			}
		}
	}
}
