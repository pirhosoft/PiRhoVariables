using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PiRhoSoft.Variables
{
	public class AggregateDictionary : IVariableDictionary
	{
		private List<IVariableDictionary> _dictionaries;
		private IReadOnlyCollection<string> _names;

		public AggregateDictionary()
		{
			_dictionaries = new List<IVariableDictionary>();
			_names = new NamesCollection(_dictionaries);
		}

		public void AddVariables(IVariableDictionary variables)
		{
			_dictionaries.Add(variables);
		}

		public void RemoveVariables(IVariableDictionary variables)
		{
			_dictionaries.Remove(variables);
		}

		private class NamesCollection : IReadOnlyCollection<string>
		{
			private IEnumerable<string> _names;
			public int Count => _names.Count();
			public NamesCollection(IList<IVariableDictionary> variables) => _names = Enumerable.SelectMany(variables, dictionary => dictionary.VariableNames);
			public IEnumerator<string> GetEnumerator() => _names.GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => _names.GetEnumerator();
		}

		#region IVariableDictionary Implementation

		public IReadOnlyCollection<string> VariableNames => _names;

		public Variable GetVariable(string name)
		{
			foreach (var dictionary in _dictionaries)
			{
				var variable = dictionary.GetVariable(name);

				if (!variable.IsEmpty)
					return variable;
			}

			return Variable.Empty;
		}

		public SetVariableResult SetVariable(string name, Variable variable)
		{
			foreach (var dictionary in _dictionaries)
			{
				// Some dictionaries allow adding which this dictionary should never do.
				if (!dictionary.GetVariable(name).IsEmpty)
					return dictionary.SetVariable(name, variable);
			}

			return SetVariableResult.NotFound;
		}

		public SetVariableResult AddVariable(string name, Variable variable) => SetVariableResult.ReadOnly;
		public SetVariableResult RemoveVariable(string name) => SetVariableResult.ReadOnly;
		public SetVariableResult ClearVariables() => SetVariableResult.ReadOnly;

		#endregion
	}
}
