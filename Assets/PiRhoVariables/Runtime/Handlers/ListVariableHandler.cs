using PiRhoSoft.Utilities;

namespace PiRhoSoft.Variables
{
	internal class ListVariableHandler : VariableHandler
	{
		private const string ListCountName = "Count";
		private const string ListString = "(List)";

		protected internal override string ToString(Variable variable)
		{
			return ListString;
		}

		protected internal override void Save(Variable value, SerializedDataWriter writer)
		{
			var list = value.AsList;

			if (list != null)
			{
				writer.Writer.Write(list.VariableCount);

				for (var i = 0; i < list.VariableCount; i++)
				{
					var item = list.GetVariable(i);
					Save(item, writer);
				}
			}
			else
			{
				writer.Writer.Write(-1);
			}
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var count = reader.Reader.ReadInt32();
			var list = new VariableList();

			for (var i = 0; i < count; i++)
			{
				var item = Load(reader);
				list.Add(item);
			}

			return Variable.List(list);
		}

		protected internal override Variable Lookup(Variable owner, Variable lookup)
		{
			if (lookup.TryGetString(out var s))
			{
				if (s == ListCountName)
					return Variable.Int(owner.AsList.VariableCount);
			}
			else if (lookup.TryGetInt(out var i))
			{
				if (i >= 0 && i < owner.AsList.VariableCount)
					return owner.AsList.GetVariable(i);
			}

			return Variable.Empty;
		}

		protected internal override SetVariableResult Assign(ref Variable owner, Variable lookup, Variable value)
		{
			if (lookup.TryGetString(out var s))
			{
				if (s == ListCountName)
					return SetVariableResult.ReadOnly;
				else
					return SetVariableResult.NotFound;
			}
			else if (lookup.TryGetInt(out var i))
			{
				if (i >= 0 && i < owner.AsList.VariableCount)
					return owner.AsList.SetVariable(i, value);
				else
					return SetVariableResult.NotFound;
			}
			else
			{
				return SetVariableResult.TypeMismatch;
			}
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.TryGetList(out var list))
				return left.AsList == list;
			else
				return null;
		}
	}
}
