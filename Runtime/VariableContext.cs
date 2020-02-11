namespace PiRhoSoft.Variables
{
	public enum SetVariableResult
	{
		Success,
		NotFound,
		ReadOnly,
		TypeMismatch
	}

	public class VariableContext : AggregateDictionary
	{
		public const string SceneName = "Scene";
		public static readonly VariableDictionary GlobalDictionary = new VariableDictionary();
		public static readonly SceneDictionary SceneDictionary = new SceneDictionary();
		public static readonly VariableContext Default = CreateDefault();

		private static VariableContext CreateDefault()
		{
			var context = new VariableContext();
			context.AddDefaultVariables();
			return context;
		}

		public void AddDefaultVariables()
		{
			GlobalDictionary.Add(SceneName, Variable.Dictionary(SceneDictionary));

			AddVariables(GlobalDictionary);
		}
	}
}
