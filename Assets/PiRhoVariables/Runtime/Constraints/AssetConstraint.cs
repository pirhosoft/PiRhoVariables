using PiRhoSoft.Utilities;
using System;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class AssetConstraint : VariableConstraint
	{
		public string Label = string.Empty;

		public override VariableType Type => VariableType.Asset;

		public AssetConstraint()
		{
		}

		public AssetConstraint(string label)
		{
			Label = label;
		}

		public override Variable Generate()
		{
			return Variable.Asset(null);
		}

		public override bool IsValid(Variable value)
		{
			return value.IsAsset;
		}
	}
}
