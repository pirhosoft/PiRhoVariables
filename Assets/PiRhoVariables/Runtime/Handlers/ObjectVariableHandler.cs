using PiRhoSoft.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PiRhoSoft.Variables
{
	internal class ObjectVariableHandler : VariableHandler
	{
		private const string NullString = "(null)";
		private const string GameObjectName = "GameObject";

		protected internal override string ToString(Variable variable)
		{
			if (variable.IsNullObject)
				return NullString;
			else
				return variable.AsObject.ToString();
		}

		protected internal override void Save(Variable value, SerializedDataWriter writer)
		{
			if (value.TryGetObject<Object>(out var obj))
			{
				writer.Writer.Write(true);
				writer.SaveReference(obj);
			}
			else
			{
				writer.Writer.Write(false);
				writer.SaveInstance(value.AsObject);
			}
		}

		protected internal override Variable Load(SerializedDataReader reader)
		{
			var isObject = reader.Reader.ReadBoolean();

			if (isObject)
			{
				var obj = reader.LoadReference();
				return Variable.Object(obj);
			}
			else
			{
				var obj = reader.LoadInstance<object>();
				return Variable.Object(obj);
			}
		}

		protected internal override Variable Lookup(Variable owner, Variable lookup)
		{
			if (owner.IsNullObject)
				return Variable.Empty;

			if (!lookup.TryGetString(out var name))
				return Variable.Empty;

			if (owner.TryGetObject<GameObject>(out var gameObject))
			{
				var child = gameObject.GetComponent(name);
				if (child != null)
					return Variable.Object(child);
			}

			if (owner.TryGetObject<Component>(out var component))
			{
				var sibling = component.gameObject.GetComponent(name);
				if (sibling != null)
					return Variable.Object(component);
			}

			var map = ObjectMap.Get(owner.ObjectType);
			if (map == null)
				return Variable.Empty;

			var property = map.GetProperty(name);
			if (property == null)
				return Variable.Empty;

			return property.Lookup(owner);
		}

		protected internal override SetVariableResult Assign(ref Variable owner, Variable lookup, Variable value)
		{
			if (owner.IsNullObject)
				return SetVariableResult.NotFound;

			if (!lookup.TryGetString(out var name))
				return SetVariableResult.NotFound;

			var map = ObjectMap.Get(owner.ObjectType);
			if (map == null)
				return SetVariableResult.TypeMismatch;

			var property = map.GetProperty(name);
			if (property == null)
				return SetVariableResult.NotFound;

			return property.Assign(owner, value);
		}

		protected internal override bool? IsEqual(Variable left, Variable right)
		{
			if (right.IsEmpty || right.IsNullObject)
				return left.IsNullObject;
			else if (right.TryGetObject(out var obj))
				return left.AsObject == obj;
			else
				return null;
		}
	}
}
