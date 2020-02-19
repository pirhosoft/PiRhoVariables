using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PiRhoSoft.Variables
{
	public interface IVariableDictionary
	{
		IReadOnlyCollection<string> VariableNames { get; }
		Variable GetVariable(string name);
		SetVariableResult SetVariable(string name, Variable variable);
		SetVariableResult AddVariable(string name, Variable variable);
		SetVariableResult RemoveVariable(string name);
		SetVariableResult ClearVariables();
	}

	public class VariableDictionary : IDictionary<string, Variable>, IVariableDictionary
	{
		private IDictionary<string, Variable> _variables;
		private IReadOnlyCollection<string> _names;

		public VariableDictionary() => SetVariables(new Dictionary<string, Variable>());
		public VariableDictionary(IDictionary<string, Variable> variables) => SetVariables(variables);

		#region Saving/Loading

		public void LoadFrom(IVariableDictionary map)
		{
			foreach (var name in map.VariableNames)
			{
				var variable = map.GetVariable(name);
				SetVariable(name, variable);
			}
		}

		public void SaveTo(IVariableDictionary map)
		{
			foreach (var name in VariableNames)
			{
				var variable = GetVariable(name);
				map.SetVariable(name, variable);
			}
		}

		#endregion


		private void SetVariables(IDictionary<string, Variable> variables)
		{
			_variables = variables;
			_names = new ReadOnlyDictionary<string, Variable>(variables).Keys;
		}

		#region IVariableDictionary Implementation

		public IReadOnlyCollection<string> VariableNames => _names;

		public Variable GetVariable(string name)
		{
			return _variables.TryGetValue(name, out var variable)
				? variable
				: Variable.Empty;
		}

		public SetVariableResult SetVariable(string name, Variable variable)
		{
			if (!_variables.ContainsKey(name))
				return SetVariableResult.NotFound;

			_variables[name] = variable;
			return SetVariableResult.Success;
		}

		public SetVariableResult AddVariable(string name, Variable variable)
		{
			if (IsReadOnly || _variables.ContainsKey(name))
				return SetVariableResult.ReadOnly;

			_variables.Add(name, variable);
			return SetVariableResult.Success;
		}

		public SetVariableResult RemoveVariable(string name)
		{
			if (IsReadOnly)
				return SetVariableResult.ReadOnly;

			return _variables.Remove(name)
				? SetVariableResult.Success
				: SetVariableResult.NotFound;
		}

		public SetVariableResult ClearVariables()
		{
			if (IsReadOnly)
				return SetVariableResult.ReadOnly;

			_variables.Clear();
			return SetVariableResult.Success;
		}

		#endregion

		#region IDictionary<string, Variable> Implementation

		public int Count => _variables.Count;
		public ICollection<string> Keys => _variables.Keys;
		public ICollection<Variable> Values => _variables.Values;
		public bool IsReadOnly => _variables.IsReadOnly;

		public Variable this[string key]
		{
			get => _variables[key];
			set => _variables[key] = value;
		}

		public bool Contains(KeyValuePair<string, Variable> item) => _variables.Contains(item);
		public bool ContainsKey(string key) => _variables.ContainsKey(key);
		public bool TryGetValue(string key, out Variable value) => _variables.TryGetValue(key, out value);
		public void Add(string key, Variable value) => _variables.Add(key, value);
		public void Add(KeyValuePair<string, Variable> item) => _variables.Add(item);
		public bool Remove(string key) => _variables.Remove(key);
		public bool Remove(KeyValuePair<string, Variable> item) => _variables.Remove(item);
		public void Clear() => _variables.Clear();
		public void CopyTo(KeyValuePair<string, Variable>[] array, int arrayIndex) => _variables.CopyTo(array, arrayIndex);
		public IEnumerator<KeyValuePair<string, Variable>> GetEnumerator() => _variables.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _variables.GetEnumerator();

		#endregion
	}
}
