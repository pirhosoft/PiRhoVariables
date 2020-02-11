using PiRhoSoft.Utilities;
using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class VariableDefinition
	{
		public string Name;
		[SerializeField] private VariableType _type;
		[SerializeReference] private VariableConstraint _constraint;
		[SerializeField] private SerializedVariable _defaultValue = new SerializedVariable();

		public VariableDefinition()
		{
			Name = string.Empty;
			Type = VariableType.Empty;
		}

		public VariableDefinition(string name)
		{
			Name = name;
			Type = VariableType.Empty;
		}

		public VariableDefinition(string name, VariableType type)
		{
			Name = name;
			Type = type;
		}

		public VariableDefinition(string name, VariableConstraint constraint)
		{
			Name = name;
			Constraint = constraint;
		}

		public string Description
		{
			get
			{
				if (Constraint == null)
					return Type.ToString();
				else
					return string.Format($"{Type}({Constraint})");
			}
		}

		public VariableType Type
		{
			get => _type;
			set
			{
				_type = value;
				_constraint = VariableConstraint.Create(value);
				_defaultValue.Variable = Generate();
			}
		}

		public VariableConstraint Constraint
		{
			get => _constraint;
			set
			{
				_type = value?.Type ?? VariableType.Empty;
				_constraint = value;
				_defaultValue.Variable = Generate();
			}
		}

		public Variable DefaultValue
		{
			get => _defaultValue.Variable;
			set => _defaultValue.Variable = value;
		}

		public Variable Generate()
		{
			return _constraint != null
				? _constraint.Generate()
				: Variable.Create(Type);
		}

		public bool IsValid(Variable variable)
		{
			return _constraint != null
				? _constraint.IsValid(variable)
				: variable.Is(Type);
		}
	}

	[Serializable]
	public class VariableDefinitionList : SerializedList<VariableDefinition>
	{
	}
}
