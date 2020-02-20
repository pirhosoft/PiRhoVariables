using PiRhoSoft.Utilities;
using PiRhoSoft.Utilities.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	public class SerializedVariableField : SerializedDataField<Variable>
	{
		private const string _dataPropertyName = "_data";

		public VariableControl Control { get; private set; }

		public SerializedVariableField(SerializedProperty property, VariableDefinition definition) : base(property.FindPropertyRelative(_dataPropertyName), Variable.Empty)
		{
			Control = new VariableControl(definition);
			Control.RegisterCallback<ChangeEvent<Variable>>(evt => Inject(evt.newValue));

			Add(Control);
			Extract();
		}

		protected override void Load(SerializedDataReader reader, ref Variable value) => value = Variable.Load(reader);
		protected override void Save(SerializedDataWriter writer, Variable value) => value.Save(writer);
		protected override void Update(Variable value) => Control.SetValueWithoutNotify(value);
	}
}
