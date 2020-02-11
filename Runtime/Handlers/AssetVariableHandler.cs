using PiRhoSoft.Utilities;
using UnityEngine.AddressableAssets;

namespace PiRhoSoft.Variables
{
	internal class AssetVariableHandler : VariableHandler
	{
		protected internal override string ToString(Variable value)
		{
			return value.AsAsset.ToString();
		}

		protected internal override void Save(Variable value, SerializedDataWriter writer)
		{
			var asset = value.AsAsset;
			var set = asset.RuntimeKeyIsValid();

			writer.Writer.Write(set);

			if (set)
				writer.Writer.Write(asset.RuntimeKey.ToString());
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var set = reader.Reader.ReadBoolean();

			if (set)
			{
				var guid = reader.Reader.ReadString();
				return Variable.Asset(new AssetReference(guid));
			}

			return Variable.Asset(null);
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.IsEmpty || right.IsNullObject)
				return !left.AsAsset.RuntimeKeyIsValid();
			else if (right.TryGetAsset(out var asset))
				return left.AsAsset.RuntimeKey.Equals(asset.RuntimeKey);
			else
				return null;
		}
	}
}
