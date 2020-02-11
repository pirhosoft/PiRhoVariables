using PiRhoSoft.Utilities;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	[CreateAssetMenu(menuName = "PiRho Composition/Variable Schema", fileName = nameof(VariableSchema), order = 112)]
	public class VariableSchema : ScriptableObject
	{
		[List]
		public VariableDefinitionList Definitions = new VariableDefinitionList();

		public bool IsImplementedBy(IVariableDictionary variables, bool exact)
		{
			if (exact && Definitions.Count != variables.VariableNames.Count)
				return false;

			foreach (var definition in Definitions)
			{
				var variable = variables.GetVariable(definition.Name);
				if (!definition.IsValid(variable))
					return false;
			}

			return true;
		}

		public void ApplyTo(IVariableDictionary variables)
		{
			foreach (var definition in Definitions)
			{
				var variable = definition.Generate();
				variables.SetVariable(definition.Name, variable);
			}
		}
	}
}
