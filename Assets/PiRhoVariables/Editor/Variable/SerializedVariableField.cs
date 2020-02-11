using PiRhoSoft.Utilities;
using PiRhoSoft.Utilities.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	public class SerializedVariableField : SerializedDataField<Variable>
	{
		private const string _dataPropertyName = "_data";

		private Label _label;
		public VariableControl Control { get; private set; }

		public SerializedVariableField(SerializedProperty property, VariableDefinition definition) : base(property.FindPropertyRelative(_dataPropertyName), Variable.Empty)
		{
			_label = new Label(property.displayName);
			_label.AddToClassList(BaseField<int>.labelUssClassName);
			Control = new VariableControl(definition);
			Control.RegisterCallback<ChangeEvent<Variable>>(evt => Inject(evt.newValue));

			Add(_label);
			Add(Control);
			style.flexDirection = FlexDirection.Row;

			Extract();
		}

		protected override void Load(SerializedDataReader reader, ref Variable value) => value = Variable.Load(reader);
		protected override void Save(SerializedDataWriter writer, Variable value) => value.Save(writer);
		protected override void Update(Variable value) => Control.SetValueWithoutNotify(value);
	}
}
