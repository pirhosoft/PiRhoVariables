using PiRhoSoft.Utilities;
using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class SerializedVariable : ISerializationCallbackReceiver
	{
		public Variable Variable = Variable.Empty;
		[SerializeField] private SerializedData _data = new SerializedData();

		#region ISerializationCallbackReceiver Implementation

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (_data.IsEmpty)
			{
				Variable = Variable.Empty;
			}
			else
			{
				using (var reader = new SerializedDataReader(_data))
					Variable = Variable.Load(reader);
			}
		}

		#endregion
	}

	[Serializable]
	public class SerializedVariableList : VariableList, ISerializationCallbackReceiver
	{
		[SerializeField] private SerializedData _data = new SerializedData();

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			ClearVariables();

			if (!_data.IsEmpty)
			{
				using (var reader = new SerializedDataReader(_data))
				{
					var count = reader.Reader.ReadInt32();

					for (var i = 0; i < count; i++)
					{
						var variable = Variable.Load(reader);
						AddVariable(variable);
					}
				}
			}
		}
	}

	[Serializable]
	public class SerializedVariableDictionary : VariableDictionary, ISerializationCallbackReceiver
	{
		[SerializeField] private SerializedData _data = new SerializedData();

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			ClearVariables();

			if (!_data.IsEmpty)
			{
				using (var reader = new SerializedDataReader(_data))
				{
					var count = reader.Reader.ReadInt32();

					for (var i = 0; i < count; i++)
					{
						var name = reader.Reader.ReadString();
						var variable = Variable.Load(reader);

						AddVariable(name, variable);
					}
				}
			}
		}
	}
}
