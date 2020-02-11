using PiRhoSoft.Utilities;
using PiRhoSoft.Utilities.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	public class SerializedVariableListField : BindableElement
	{
		private const string _dataPropertyName = "_data";

		private VariableList _list = new VariableList();

		public SerializedVariableListField(SerializedProperty property)
		{
			var proxy = new DataProxy(_list);
			var field = new ListField();
			var dataProperty = property.FindPropertyRelative(_dataPropertyName);
			var data = new DataField(dataProperty, _list, field);

			field.Label = property.displayName;
			field.Tooltip = property.GetTooltip();
			field.SetProxy(proxy, null, false);
			field.RegisterCallback<ListField.ItemAddedEvent>(e => data.Inject(_list));
			field.RegisterCallback<ListField.ItemRemovedEvent>(e => data.Inject(_list));
			field.RegisterCallback<ChangeEvent<Variable>>(e => data.Inject(_list));

			Add(data);
			Add(field);
		}

		private class DataField : SerializedDataField<IVariableList>
		{
			public ListField Field;

			public DataField(SerializedProperty property, IVariableList List, ListField field) : base(property, List)
			{
				Field = field;
				Extract();
			}

			protected override void Load(SerializedDataReader reader, ref IVariableList value)
			{
				value.ClearVariables();

				var count = reader.Reader.ReadInt32();

				for (var i = 0; i < count; i++)
				{
					var variable = Variable.Load(reader);
					value.AddVariable(variable);
				}
			}

			protected override void Save(SerializedDataWriter writer, IVariableList value)
			{
				writer.Writer.Write(value.VariableCount);

				for (var i = 0; i < value.VariableCount; i++)
				{
					var variable = value.GetVariable(i);
					variable.Save(writer);
				}
			}

			protected override void Update(IVariableList value)
			{
				Field.Rebuild();
			}
		}

		private class DataProxy : IListProxy
		{
			public IVariableList List;

			public int Count => List.VariableCount;
			public bool IsReorderable => true;

			public bool CanAdd() => true;
			public bool CanAdd(Type type) => true;
			public bool CanRemove(int index) => true;

			public DataProxy(IVariableList list)
			{
				List = list;
			}

			public bool AddItem(Type type)
			{
				return List.AddVariable(Variable.Empty) == SetVariableResult.Success;
			}

			public void RemoveItem(int index)
			{
				List.RemoveVariable(index);
			}

			public void ReorderItem(int from, int to)
			{
				var fromVariable = List.GetVariable(from);
				var toVariable = List.GetVariable(to);

				List.SetVariable(to, fromVariable);
				List.SetVariable(from, toVariable);
			}

			public VisualElement CreateElement(int index)
			{
				var variable = List.GetVariable(index);
				var control = new VariableControl(null);

				control.RegisterCallback<ChangeEvent<Variable>>(e =>
				{
					List.SetVariable(index, e.newValue);
				});

				control.SetValueWithoutNotify(variable);
				return control;
			}
		}
	}
}
