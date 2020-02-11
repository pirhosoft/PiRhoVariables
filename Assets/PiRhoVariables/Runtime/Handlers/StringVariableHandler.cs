using PiRhoSoft.Utilities;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	internal class StringVariableHandler : VariableHandler
	{
		public const char Symbol = '\"';

		protected internal override string ToString(Variable variable)
		{
			return variable.AsString;
		}

		protected internal override void Save(Variable variable, SerializedDataWriter writer)
		{
			writer.Writer.Write(variable.AsString);
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var s = reader.Reader.ReadString();
			return Variable.String(s);
		}

		protected internal override Variable Add(Variable left, Variable right)
		{
			return Variable.String(left.AsString + right.ToString());
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.TryGetString(out var str))
				return left.AsString == str;
			else
				return null;
		}

		protected internal override int? Compare(Variable left, Variable right)
		{
			if (right.TryGetString(out var s))
				return left.AsString.CompareTo(s);
			else
				return null;
		}

		protected internal override float Distance(Variable from, Variable to)
		{
			if (to.TryGetString(out var t))
				return Mathf.Max(from.AsString.Length, t.Length);
			else
				return 0.0f;
		}

		protected internal override Variable Interpolate(Variable from, Variable to, float time)
		{
			if (to.TryGetString(out var t))
			{
				var f = from.AsString;
				var total = Mathf.Max(f.Length, t.Length);
				var length = (int)Mathf.Lerp(0, total, time);

				if (f.Length < length) f = f.PadRight(length);
				if (t.Length < length) t = t.PadRight(length);

				var start = f.Substring(0, length);
				var end = t.Substring(length);

				return Variable.String(start + end);
			}
			else
			{
				return Variable.Empty;
			}
		}
	}
}
