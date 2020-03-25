using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PiRhoSoft.Variables
{
	public interface IVariableHierarchy : IVariableDictionary
	{
	}

	public class ChildDictionary : IVariableDictionary
	{
		private IVariableDictionary _parent;
		private IVariableDictionary _child;
		private IReadOnlyCollection<string> _names;

		public ChildDictionary(IVariableDictionary parent)
		{
			_parent = parent;
			_child = new VariableDictionary();
			_names = new NamesCollection(_child, _parent);
		}

		public ChildDictionary(IVariableDictionary parent, IVariableDictionary child)
		{
			_parent = parent;
			_child = child;
			_names = new NamesCollection(_child, _parent);
		}

		private class NamesCollection : IReadOnlyCollection<string>
		{
			private IEnumerable<string> _names;
			public int Count => _names.Count();
			public NamesCollection(IVariableDictionary self, IVariableDictionary parent) => _names = Enumerable.Union(self.VariableNames, parent.VariableNames);
			public IEnumerator<string> GetEnumerator() => _names.GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => _names.GetEnumerator();
		}

		#region IVariableDictionary Implementation

		public IReadOnlyCollection<string> VariableNames => _names;

		public Variable GetVariable(string name)
		{
			var self = _child.GetVariable(name);

			return self.IsEmpty
				? _parent.GetVariable(name)
				: self;
		}

		public SetVariableResult SetVariable(string name, Variable variable)
		{
			// The _parent dictionary might allow arbitrary adds so only set it on the parent if it already exists (as
			// opposed to checking for a set failure).
			var exists = !_parent.GetVariable(name).IsEmpty;

			if (exists)
				return _parent.SetVariable(name, variable);
			else
				return _child.SetVariable(name, variable);
		}

		public SetVariableResult AddVariable(string name, Variable variable) => _child.AddVariable(name, variable);
		public SetVariableResult RemoveVariable(string name) => _child.RemoveVariable(name);
		public SetVariableResult ClearVariables() => _child.ClearVariables();

		#endregion
	}
}
