using PiRhoSoft.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class StringConstraintValueList : SerializedList<string> { }

	[Serializable]
	public class StringConstraint : VariableConstraint
	{
		public override VariableType Type => VariableType.String;

		public List<string> Values { get => _values.List; set => SetValues(value); }
		[SerializeField] [List] private StringConstraintValueList _values = new StringConstraintValueList();

		public StringConstraint()
		{
		}

		public StringConstraint(IList<string> values)
		{
			SetValues(values);
		}

		public override string ToString()
		{
			return string.Join(",", Values);
		}

		public override Variable Generate()
		{
			return Values.Count > 0
				? Variable.String(Values[0])
				: Variable.String(string.Empty);
		}

		public override bool IsValid(Variable value)
		{
			return value.IsString && (Values.Count == 0 || Values.Contains(value.AsString));
		}

		private void SetValues(IList<string> values)
		{
			_values.Clear();

			if (values != null)
			{
				foreach (var value in values)
					_values.Add(value);
			}
		}
	}
}
