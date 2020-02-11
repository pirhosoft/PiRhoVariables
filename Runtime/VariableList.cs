using System.Collections;
using System.Collections.Generic;

namespace PiRhoSoft.Variables
{
	public interface IVariableList
	{
		int VariableCount { get; }
		Variable GetVariable(int index);
		SetVariableResult SetVariable(int index, Variable variable);
		SetVariableResult AddVariable(Variable variable);
		SetVariableResult InsertVariable(int index, Variable variable);
		SetVariableResult RemoveVariable(int index);
		SetVariableResult ClearVariables();
	}

	public class VariableList : IList<Variable>, IVariableList
	{
		private IList<Variable> _variables;

		public VariableList() => _variables = new List<Variable>();
		public VariableList(IList<Variable> variables) => _variables = variables;

		#region IVariableList Implementation

		public int VariableCount => _variables.Count;

		public Variable GetVariable(int index)
		{
			return index >= 0 && index < Count
				? this[index]
				: Variable.Empty;
		}

		public SetVariableResult SetVariable(int index, Variable variable)
		{
			if (index < 0 && index >= Count)
				return SetVariableResult.NotFound;

			this[index] = variable;
			return SetVariableResult.Success;
		}

		public SetVariableResult AddVariable(Variable variable)
		{
			if (IsReadOnly)
				return SetVariableResult.ReadOnly;

			Add(variable);
			return SetVariableResult.Success;
		}

		public SetVariableResult InsertVariable(int index, Variable variable)
		{
			if (IsReadOnly)
				return SetVariableResult.ReadOnly;
			else if (index < 0 || index > Count)
				return SetVariableResult.NotFound;

			Insert(index, variable);
			return SetVariableResult.Success;
		}

		public SetVariableResult RemoveVariable(int index)
		{
			if (IsReadOnly)
				return SetVariableResult.ReadOnly;
			else if (index < 0 || index >= Count)
				return SetVariableResult.NotFound;

			RemoveAt(index);
			return SetVariableResult.Success;
		}

		public SetVariableResult ClearVariables()
		{
			if (IsReadOnly)
				return SetVariableResult.ReadOnly;

			Clear();
			return SetVariableResult.Success;
		}

		#endregion

		#region IList<Variable> Implementation

		public int Count => _variables.Count;
		public bool IsReadOnly => _variables.IsReadOnly;

		public Variable this[int index]
		{
			get => _variables[index];
			set => _variables[index] = value;
		}

		public int IndexOf(Variable item) => _variables.IndexOf(item);
		public bool Contains(Variable item) => _variables.Contains(item);
		public void Add(Variable item) => _variables.Add(item);
		public void Insert(int index, Variable item) => _variables.Insert(index, item);
		public bool Remove(Variable item) => _variables.Remove(item);
		public void RemoveAt(int index) => _variables.RemoveAt(index);
		public void Clear() => _variables.Clear();
		public void CopyTo(Variable[] array, int arrayIndex) => _variables.CopyTo(array, arrayIndex);
		public IEnumerator<Variable> GetEnumerator() => _variables.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _variables.GetEnumerator();

		#endregion
	}
}
