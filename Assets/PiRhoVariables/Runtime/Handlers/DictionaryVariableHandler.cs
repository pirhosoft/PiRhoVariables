using PiRhoSoft.Utilities;

namespace PiRhoSoft.Variables
{
	internal class DictionaryVariableHandler : VariableHandler
	{
		private const string DictionaryString = "(Dictionary)";

		protected internal override string ToString(Variable variable)
		{
			return DictionaryString;
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			var dictionary = variable.AsDictionary;
			var names = dictionary.VariableNames;

			writer.Writer.Write(names.Count);

			foreach (var name in names)
			{
				var value = dictionary.GetVariable(name);

				writer.Writer.Write(name);
				Save(value, writer);
			}
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var dictionary = new VariableDictionary();
			var count = reader.Reader.ReadInt32();

			for (var i = 0; i < count; i++)
			{
				var name = reader.Reader.ReadString();
				var value = Load(reader);

				dictionary.Add(name, value);
			}

			return Variable.Dictionary(dictionary);
		}

		protected internal override Variable Lookup(Variable owner, Variable lookup)
		{
			if (lookup.TryGetString(out var name))
				return owner.AsDictionary.GetVariable(name);

			return Variable.Empty;
		}

		protected internal override SetVariableResult Assign(ref Variable owner, Variable lookup, Variable value)
		{
			if (lookup.TryGetString(out var s))
				return owner.AsDictionary.SetVariable(s, value);
			else
				return SetVariableResult.TypeMismatch;
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.TryGetDictionary(out var dictionary))
				return left.AsDictionary == dictionary;
			else
				return null;
		}
	}
}
