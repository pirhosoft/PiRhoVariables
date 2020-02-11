using System;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class DictionaryConstraint : VariableConstraint
	{
		public VariableSchema Schema;

		public override VariableType Type => VariableType.Dictionary;

		public DictionaryConstraint()
		{
		}

		public DictionaryConstraint(VariableSchema schema)
		{
			Schema = schema;
		}

		public override string ToString()
		{
			return Schema?.name ?? string.Empty;
		}

		public override Variable Generate()
		{
			var dictionary = new VariableDictionary();

			if (Schema != null)
				Schema.ApplyTo(dictionary);

			return Variable.Dictionary(dictionary);
		}

		public override bool IsValid(Variable variable)
		{
			return variable.IsDictionary && (Schema == null || Schema.IsImplementedBy(variable.AsDictionary, false));
		}
	}
}
