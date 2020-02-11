using PiRhoSoft.Utilities;
using PiRhoSoft.Utilities.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	public class SerializedVariableDictionaryField : BindableElement
	{
		private const string _dataPropertyName = "_data";

		private VariableDictionary _dictionary = new VariableDictionary();

		public SerializedVariableDictionaryField(SerializedProperty property)
		{
			var proxy = new DataProxy(_dictionary);

			var field = new DictionaryField();
			field.Label = property.displayName;
			field.Tooltip = property.GetTooltip();
			field.SetProxy(proxy, null, false);

			var dataProperty = property.FindPropertyRelative(_dataPropertyName);
			var data = new DataField(dataProperty, _dictionary, field);

			field.RegisterCallback<DictionaryField.ItemAddedEvent>(e => data.Inject(_dictionary));
			field.RegisterCallback<DictionaryField.ItemRemovedEvent>(e => data.Inject(_dictionary));
			field.RegisterCallback<ChangeEvent<Variable>>(e => data.Inject(_dictionary));

			Add(data);
			Add(field);
		}

		private class DataField : SerializedDataField<IVariableDictionary>
		{
			public DictionaryField Field;

			public DataField(SerializedProperty property, IVariableDictionary dictionary, DictionaryField field) : base(property, dictionary)
			{
				Field = field;
				Extract();
			}

			protected override void Load(SerializedDataReader reader, ref IVariableDictionary value)
			{
				value.ClearVariables();

				var count = reader.Reader.ReadInt32();

				for (var i = 0; i < count; i++)
				{
					var name = reader.Reader.ReadString();
					var variable = Variable.Load(reader);

					value.AddVariable(name, variable);
				}
			}

			protected override void Save(SerializedDataWriter writer, IVariableDictionary value)
			{
				writer.Writer.Write(value.VariableNames.Count);

				foreach (var name in value.VariableNames)
				{
					var variable = value.GetVariable(name);
					writer.Writer.Write(name);
					variable.Save(writer);
				}
			}

			protected override void Update(IVariableDictionary value)
			{
				Field.Rebuild();
			}
		}

		private class DataProxy : IDictionaryProxy
		{
			public IVariableDictionary Dictionary;

			public int Count => Dictionary.VariableNames.Count;
			public bool IsReorderable => false;

			public bool CanAdd(string key) => !Dictionary.VariableNames.Contains(key);
			public bool CanAdd(Type type) => true;
			public bool CanRemove(int index, string key) => true;
			public void ReorderItem(int from, int to) { }

			public DataProxy(IVariableDictionary dictionary)
			{
				Dictionary = dictionary;
			}

			public string GetKey(int index)
			{
				return Dictionary.VariableNames.ElementAt(index);
			}

			public bool AddItem(string key, Type type)
			{
				return Dictionary.AddVariable(key, Variable.Empty) == SetVariableResult.Success;
			}

			public void RemoveItem(int index, string key)
			{
				Dictionary.RemoveVariable(key);
			}

			public VisualElement CreateElement(int index, string key)
			{
				var variable = Dictionary.GetVariable(key);
				var container = new FieldContainer(key);
				var control = new VariableControl(null);

				control.RegisterCallback<ChangeEvent<Variable>>(e =>
				{
					Dictionary.SetVariable(key, e.newValue);
				});

				control.SetValueWithoutNotify(variable);
				container.Add(control);

				return container;
			}
		}
	}
}
