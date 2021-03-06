﻿using PiRhoSoft.Utilities;
using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	internal class EnumVariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable variable)
		{
			return variable.AsEnum.ToString();
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			writer.SaveEnum(variable.AsEnum);
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var e = reader.LoadEnum();
			return Variable.Enum(e);
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.TryGetEnum(left.EnumType, out var e))
				return left.AsEnum == e;
			else
				return null;
		}

		protected internal override int? Compare(Variable left, Variable right)
		{
			if (right.TryGetEnum(left.EnumType, out var e))
				return left.AsEnum.CompareTo(e);
			else
				return null;
		}

		protected internal override float Distance(Variable from, Variable to)
		{
			if (to.TryGetEnum(from.EnumType, out var t))
			{
				var fInt = (int)Enum.Parse(from.EnumType, from.AsEnum.ToString());
				var tInt = (int)Enum.Parse(to.EnumType, t.ToString());

				return tInt = fInt;
			}
			else
			{
				return 0.0f;
			}
		}

		protected internal override Variable Interpolate(Variable from, Variable to, float time)
		{
			if (to.TryGetEnum(from.EnumType, out var t))
			{
				var fInt = (int)Enum.Parse(from.EnumType, from.AsEnum.ToString());
				var tInt = (int)Enum.Parse(to.EnumType, t.ToString());
				var value = Mathf.Lerp(fInt, tInt, time);

				return Variable.Int((int)value);
			}
			else
			{
				return Variable.Empty;
			}
		}
	}
}
