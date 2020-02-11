using PiRhoSoft.Utilities;

namespace PiRhoSoft.Variables
{
	internal abstract class VariableHandler
	{
		private static VariableHandler[] _handlers = new VariableHandler[]
		{
			new EmptyVariableHandler(),
			new BoolVariableHandler(),
			new IntVariableHandler(),
			new FloatVariableHandler(),
			new Vector2IntVariableHandler(),
			new Vector3IntVariableHandler(),
			new RectIntVariableHandler(),
			new BoundsIntVariableHandler(),
			new Vector2VariableHandler(),
			new Vector3VariableHandler(),
			new Vector4VariableHandler(),
			new QuaternionVariableHandler(),
			new RectVariableHandler(),
			new BoundsVariableHandler(),
			new ColorVariableHandler(),
			new EnumVariableHandler(),
			new StringVariableHandler(),
			new ListVariableHandler(),
			new DictionaryVariableHandler(),
			new AssetVariableHandler(),
			new ObjectVariableHandler(),
			new FunctionVariableHandler()
		};

		public static VariableHandler Get(VariableType type) => _handlers[(int)type];

		protected internal abstract string ToString(Variable value);

		protected internal abstract void Save(Variable value, SerializedDataWriter writer);
		protected internal abstract Variable Load(SerializedDataReader reader);

		protected internal virtual Variable Add(Variable left, Variable right) => Variable.Empty;
		protected internal virtual Variable Subtract(Variable left, Variable right) => Variable.Empty;
		protected internal virtual Variable Multiply(Variable left, Variable right) => Variable.Empty;
		protected internal virtual Variable Divide(Variable left, Variable right) => Variable.Empty;
		protected internal virtual Variable Modulo(Variable left, Variable right) => Variable.Empty;
		protected internal virtual Variable Exponent(Variable left, Variable right) => Variable.Empty;
		protected internal virtual Variable Negate(Variable value) => Variable.Empty;

		protected internal virtual bool? IsEqual(Variable left, Variable right) => null;
		protected internal virtual int? Compare(Variable left, Variable right) => null;

		protected internal virtual float Distance(Variable from, Variable to) => 0.0f;
		protected internal virtual Variable Interpolate(Variable from, Variable to, float time) => Variable.Empty;

		protected internal virtual Variable Lookup(Variable owner, Variable lookup) => Variable.Empty;
		protected internal virtual SetVariableResult Assign(ref Variable owner, Variable lookup, Variable value) => SetVariableResult.NotFound;
	}
}
