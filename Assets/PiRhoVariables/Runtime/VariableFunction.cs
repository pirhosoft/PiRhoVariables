using System;

namespace PiRhoSoft.Variables
{
	public interface IVariableFunction
	{
		int Validate(Variable[] parameters);
		Variable Invoke(Variable[] parameters);
	}

	public class VariableFunction : IVariableFunction
	{
		public const int Valid = int.MinValue;
		public const int NotFound = int.MaxValue - 1;
		public const int IncorrectLength = int.MaxValue;

		private Delegate _delegate;
		private object[] _arguments;

		public VariableFunction(Delegate d)
		{
			// TODO: Check for null?

			var parameters = d.Method.GetParameters();

			_delegate = d;
			_arguments = new object[parameters.Length];
		}

		public int Validate(Variable[] arguments)
		{
			var parameters = _delegate.Method.GetParameters();

			if (arguments.Length != parameters.Length)
				return IncorrectLength;

			for (var i = 0; i < arguments.Length; i++)
			{
				if (!arguments[i].Is(parameters[i].ParameterType))
					return i;
			}

			return Valid;
		}

		public Variable Invoke(Variable[] arguments)
		{
			var check = Validate(arguments);

			if (check != Valid)
				throw new VariableFunctionException();

			return UncheckedInvoke(arguments);
		}

		public Variable UncheckedInvoke(Variable[] arguments)
		{
			for (var i = 0; i < arguments.Length; i++)
				_arguments[i] = arguments[i].Box();

			var result = _delegate.DynamicInvoke(_arguments);
			return Variable.Unbox(result);
		}
	}

	public class VariableOverload : IVariableFunction
	{
		private IVariableFunction[] _functions;

		public VariableOverload(params Delegate[] functions)
		{
			_functions = new IVariableFunction[functions.Length];

			for (var i = 0; i < functions.Length; i++)
				_functions[i] = new VariableFunction(functions[i]);
		}

		public int Validate(Variable[] parameters)
		{
			foreach (var function in _functions)
			{
				if (function.Validate(parameters) == VariableFunction.Valid)
					return VariableFunction.Valid;
			}

			return VariableFunction.NotFound;
		}

		public Variable Invoke(Variable[] parameters)
		{
			foreach (var function in _functions)
			{
				if (function.Validate(parameters) == VariableFunction.Valid)
					return function.Invoke(parameters);
			}

			throw new VariableFunctionException();
		}
	}

	public class VariableFunctionException : Exception
	{
		// TODO: Include the expected and passed in signatures.
		public VariableFunctionException() : base("failed to invoke function") { }
	}
}
