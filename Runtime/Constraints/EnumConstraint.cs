using PiRhoSoft.Utilities;
using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	// TODO: Could do something fancy here where any enum type that has a definition gets a cache created holding its
	// boxed values so they can be looked up (as opposed to doing a boxing conversion) at runtime.

	[Serializable]
	public class EnumConstraint : VariableConstraint
	{
		public override VariableType Type => VariableType.Enum;

		[SerializeField]
		[TypePicker(typeof(Enum), false)]
		private string _enumType;

		public Type EnumType
		{
			get => System.Type.GetType(_enumType);
			set => _enumType = (Variable.IsValidEnumType(value) ? value : typeof(Variable.InvalidEnum)).AssemblyQualifiedName;
		}

		public EnumConstraint()
		{
			EnumType = null;
		}

		public EnumConstraint(Type type)
		{
			EnumType = type;
		}

		public override string ToString()
		{
			return EnumType.Name;
		}

		public override Variable Generate()
		{
			var values = EnumType.GetEnumValues();
			return Variable.Enum(values.GetValue(0) as Enum);
		}

		public override bool IsValid(Variable variable)
		{
			return variable.HasEnum(EnumType);
		}
	}
}
