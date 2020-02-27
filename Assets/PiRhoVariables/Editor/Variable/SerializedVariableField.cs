using PiRhoSoft.Utilities;
using PiRhoSoft.Utilities.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace PiRhoSoft.Variables.Editor
{
	public class SerializedVariableField : SerializedDataField<Variable>
	{
		public const string Stylesheet = "SerializedVariableStyle.uss";
		public const string UssClassName = "pirho-serialized-variable";
		public const string InputUssClassName = UssClassName + "__input";

		private const string _dataPropertyName = "_data";

		public VariableControl Control { get; private set; }

		public SerializedVariableField(SerializedProperty property, VariableDefinition definition) : base(property.FindPropertyRelative(_dataPropertyName), Variable.Empty)
		{
			Control = new VariableControl(definition);
			Control.AddToClassList(InputUssClassName);
			Control.RegisterCallback<ChangeEvent<Variable>>(evt => Inject(evt.newValue));

			AddToClassList(UssClassName);
			Add(Control);
			Extract();

			this.AddStyleSheet(Stylesheet);
		}

		protected override void Load(SerializedDataReader reader, ref Variable value) => value = Variable.Load(reader);
		protected override void Save(SerializedDataWriter writer, Variable value) => value.Save(writer);
		protected override void Update(Variable value) => Control.SetValueWithoutNotify(value);
	}
}
