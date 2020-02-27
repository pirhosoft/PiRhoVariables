using PiRhoSoft.Utilities;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class ObjectConstraint : VariableConstraint
	{
		public override VariableType Type => VariableType.Object;

		[SerializeField]
		private string _objectType;

		public Type ObjectType
		{
			get => System.Type.GetType(_objectType);
			set => _objectType = (value ?? typeof(Object)).AssemblyQualifiedName;
		}

		public ObjectConstraint()
		{
			ObjectType = null;
		}

		public ObjectConstraint(Type type)
		{
			ObjectType = type;
		}

		public override string ToString()
		{
			return ObjectType.Name;
		}

		public override Variable Generate()
		{
			return Variable.Object(null);
		}

		public override bool IsValid(Variable variable)
		{
			return variable.IsNullObject || variable.HasObject(ObjectType);
		}
	}
}
