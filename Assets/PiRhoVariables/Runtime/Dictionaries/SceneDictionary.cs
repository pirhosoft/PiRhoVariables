using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	public class SceneDictionary : IVariableDictionary
	{
		private static List<string> _names = new List<string>();
		public static IReadOnlyCollection<string> Names = _names.AsReadOnly();

		public static void RefreshNames()
		{
			_names = Object.FindObjectsOfType<GameObject>()
				.Select(o => o.name)
				.ToList();
		}

		public IReadOnlyCollection<string> VariableNames => Names;

		public Variable GetVariable(string name)
		{
			var gameObject = GameObject.Find(name);

			// Return Empty instead of a null Object to indicate not found in the same way as other
			// IVariableDictionary implementations.
			if (gameObject == null)
				return Variable.Empty;

			return Variable.Object(gameObject);
		}

		public SetVariableResult SetVariable(string name, Variable value) => SetVariableResult.ReadOnly;
		public SetVariableResult AddVariable(string name, Variable variable) => SetVariableResult.ReadOnly;
		public SetVariableResult RemoveVariable(string name) => SetVariableResult.ReadOnly;
		public SetVariableResult ClearVariables() => SetVariableResult.ReadOnly;
	}
}
