using System;
using System.Collections.Generic;
using System.Reflection;

namespace PiRhoSoft.Variables
{
	public interface IObjectMap
	{
		IMappedProperty GetProperty(string name);
	}

	public interface IMappedProperty
	{
		Type PropertyType { get; }
		Variable Lookup(Variable owner);
		SetVariableResult Assign(Variable owner, Variable value);
	}

	public interface IMappedProperty<Type> : IMappedProperty
	{
		Type Lookup(object owner);
		SetVariableResult Assign(object owner, Type value);
	}

	public abstract class ObjectMap : IObjectMap
	{
		protected static Dictionary<Type, IObjectMap> _maps = new Dictionary<Type, IObjectMap>();

		public static IObjectMap Get(Type type)
		{
			if (!_maps.TryGetValue(type, out var map))
			{
				map = new ReflectionMap(type);
				_maps.Add(type, map);
			}

			return map;
		}

		public static void Register(Type type, IObjectMap map)
		{
			_maps.Add(type, map);
		}

		#region Base Implementation

		private Dictionary<string, IMappedProperty> _properties = new Dictionary<string, IMappedProperty>();

		public void Add(string name, IMappedProperty property)
		{
			_properties[name] = property;
		}

		IMappedProperty IObjectMap.GetProperty(string name)
		{
			return _properties.TryGetValue(name, out var property)
				? property
				: null;
		}

		protected abstract class MappedProperty<Type> : IMappedProperty<Type>
		{
			public System.Type PropertyType => typeof(Type);

			Variable IMappedProperty.Lookup(Variable owner)
			{
				var val = Lookup(owner.Box());
				return Variable.Unbox(val);
			}

			SetVariableResult IMappedProperty.Assign(Variable owner, Variable value)
			{
				if (!value.TryGet<Type>(out var val))
					return SetVariableResult.TypeMismatch;

				return Assign(owner.Box(), val);
			}

			public abstract Type Lookup(object owner);
			public abstract SetVariableResult Assign(object owner, Type value);
		}

		#endregion

		#region Reflection Implementation

		// TODO: These need to derive from IMappedProperty<PropertyType>.

		private class MappedFieldInfo<PropertyType> : MappedProperty<PropertyType>
		{
			private FieldInfo _field;

			public MappedFieldInfo(FieldInfo field)
			{
				_field = field;
			}

			public override PropertyType Lookup(object owner)
			{
				return (PropertyType)_field.GetValue(owner);
			}

			public override SetVariableResult Assign(object owner, PropertyType value)
			{
				if (_field.IsInitOnly || _field.IsLiteral)
					return SetVariableResult.ReadOnly;

				try
				{
					_field.SetValue(owner, value);
					return SetVariableResult.Success;
				}
				catch
				{
					return SetVariableResult.TypeMismatch;
				}
			}
		}

		private class MappedPropertyInfo<PropertyType> : MappedProperty<PropertyType>
		{
			private PropertyInfo _property;

			public MappedPropertyInfo(PropertyInfo property)
			{
				_property = property;
			}

			public override PropertyType Lookup(object owner)
			{
				return (PropertyType)_property.GetValue(owner);
			}

			public override SetVariableResult Assign(object owner, PropertyType value)
			{
				if (_property.GetSetMethod() == null)
					return SetVariableResult.ReadOnly;

				try
				{
					_property.SetValue(owner, value);
					return SetVariableResult.Success;
				}
				catch
				{
					return SetVariableResult.TypeMismatch;
				}
			}
		}

		private class ReflectionMap : IObjectMap
		{
			private Dictionary<string, IMappedProperty> _properties;

			public ReflectionMap(Type type)
			{
				_properties = new Dictionary<string, IMappedProperty>();

				var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
				var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

				foreach (var field in fields)
				{
					if (!_properties.ContainsKey(field.Name)) // Handle classes overwriting parent fields/properties using 'new'.
					{
						var map = Create(typeof(MappedFieldInfo<>), field.FieldType, field);
						_properties.Add(field.Name, map);
					}
				}

				foreach (var property in properties)
				{
					if (!_properties.ContainsKey(property.Name))
					{
						var map = Create(typeof(MappedPropertyInfo<>), property.PropertyType, property);
						_properties.Add(property.Name, map);
					}
				}
			}

			public IMappedProperty GetProperty(string name)
			{
				return _properties.TryGetValue(name, out var property)
					? property
					: null;
			}

			// TODO: These need to be registered to work with code stripping.

			private IMappedProperty Create(Type mapType, Type propertyType, object member)
			{
				var type = mapType.MakeGenericType(propertyType);
				return Activator.CreateInstance(type, member) as IMappedProperty;
			}
		}

		#endregion
	}

	public class ObjectMap<ObjectType> : ObjectMap
	{
		private static ObjectMap GetMap()
		{
			var type = typeof(ObjectType);

			if (!_maps.TryGetValue(type, out var map))
			{
				map = new ObjectMap<ObjectType>();
				_maps.Add(type, map);
			}

			return map as ObjectMap;
		}

		public static void Add<PropertyType>(string propertyName, Func<ObjectType, PropertyType> lookup, Action<ObjectType, PropertyType> assign)
		{
			var map = GetMap();
			map.Add(propertyName, new LambdaProperty<PropertyType>(lookup, assign));
		}

		public static void Add<PropertyType>(string propertyName, Func<ObjectType, PropertyType> lookup)
		{
			var map = GetMap();
			map.Add(propertyName, new LambdaProperty<PropertyType>(lookup, null));
		}

		#region Lambda Implementation

		private class LambdaProperty<Type> : MappedProperty<Type>
		{
			private Func<ObjectType, Type> _lookup;
			private Action<ObjectType, Type> _assign;

			public LambdaProperty(Func<ObjectType, Type> lookup, Action<ObjectType, Type> assign)
			{
				_lookup = lookup;
				_assign = assign;
			}

			public override Type Lookup(object owner)
			{
				return _lookup((ObjectType)owner);
			}

			public override SetVariableResult Assign(object owner, Type value)
			{
				if (_assign != null)
				{
					_assign((ObjectType)owner, value);
					return SetVariableResult.Success;
				}

				return SetVariableResult.ReadOnly;
			}
		}

		#endregion
	}
}
