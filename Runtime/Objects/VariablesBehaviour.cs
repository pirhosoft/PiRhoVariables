using System.Collections.Generic;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	public class VariablesBehaviour : MonoBehaviour, IVariableHierarchy
	{
		private IVariableDictionary _hierarchy;
		public SerializedVariableDictionary Variables = new SerializedVariableDictionary();

		public IReadOnlyCollection<string> VariableNames => _hierarchy.VariableNames;

		private void Awake()
		{
			var parent = transform.parent ? transform.parent.GetComponentInParent<IVariableHierarchy>() as IVariableDictionary : VariableContext.Default;
			_hierarchy = new ChildDictionary(parent ?? VariableContext.Default, Variables);
		}

		#region IVariableDictionary Implementation

		public Variable GetVariable(string name) => _hierarchy.GetVariable(name);
		public SetVariableResult SetVariable(string name, Variable variable) => _hierarchy.SetVariable(name, variable);
		public SetVariableResult AddVariable(string name, Variable variable) => _hierarchy.AddVariable(name, variable);
		public SetVariableResult RemoveVariable(string name) => _hierarchy.RemoveVariable(name);
		public SetVariableResult ClearVariables() => _hierarchy.ClearVariables();

		#endregion
	}

	public class VariableAccess : IVariableDictionary
	{
		private readonly IVariableDictionary _variables;

		public VariableAccess(MonoBehaviour sibling)
		{
			var variables = sibling.GetComponentInParent<IVariableHierarchy>() as IVariableDictionary;
			_variables = new ChildDictionary(variables ?? VariableContext.Default);
		}

		public IReadOnlyCollection<string> VariableNames => _variables.VariableNames;
		public SetVariableResult AddVariable(string name, Variable variable) => _variables.AddVariable(name, variable);
		public SetVariableResult ClearVariables() => _variables.ClearVariables();
		public Variable GetVariable(string name) => _variables.GetVariable(name);
		public SetVariableResult RemoveVariable(string name) => _variables.RemoveVariable(name);
		public SetVariableResult SetVariable(string name, Variable variable) => _variables.SetVariable(name, variable);
	}
}
