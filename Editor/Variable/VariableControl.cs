using PiRhoSoft.Utilities.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace PiRhoSoft.Variables.Editor
{
	public class VariableControl : VisualElement
	{
		public const string Stylesheet = "VariableStyle.uss";
		public const string UssClassName = "pirho-variable";
		public const string EmptyUssClassName = UssClassName + "__empty";
		public const string ResetUssClassName = UssClassName + "__reset";
		public const string BoolUssClassName = UssClassName + "__bool";
		public const string NumberUssClassName = UssClassName + "__number";
		public const string NumberHasRangeUssClassName = NumberUssClassName + "--has-range";
		public const string NumberSliderUssClassName = NumberUssClassName + "__slider";
		public const string NumberFieldUssClassName = NumberUssClassName + "__field";
		public const string FieldUssClassName = UssClassName + "__field";
		public const string ContainerUssClassName = UssClassName + "__container";
		public const string DictionaryLabelUssClassName = UssClassName + "__dictionary__label";

		private Variable _value;
		private readonly VariableDefinition _definition;
		public Variable Value => _value;

		private IconButton _resetButton;
		private EnumField _emptyField;
		private Toggle _boolToggle;
		private VisualElement _intContainer;
		private VisualElement _floatContainer;
		private Vector2IntField _vector2IntField;
		private Vector3IntField _vector3IntField;
		private RectIntField _rectIntField;
		private BoundsIntField _boundsIntField;
		private Vector2Field _vector2Field;
		private Vector3Field _vector3Field;
		private Vector4Field _vector4Field;
		private EulerField _quaternionField;
		private RectField _rectField;
		private BoundsField _boundsField;
		private ColorField _colorField;
		private EnumField _enumField;
		private VisualElement _stringContainer;
		private ListField _listField;
		private DictionaryField _dictionaryField;
		private ObjectField _assetField;
		private ObjectField _objectField;

		private IntegerField _intField;
		private SliderInt _intSlider;
		private FloatField _floatField;
		private Slider _floatSlider;
		private PopupStringField _stringPopup;
		private TextField _stringField;
		private VariableListProxy _listProxy;
		private VariableDictionaryProxy _dictionaryProxy;

		public VariableControl(VariableDefinition definition)
		{
			_definition = definition;

			CreateElements();

			this.AddStyleSheet(Stylesheet);
			AddToClassList(UssClassName);
		}

		public void SetValueWithoutNotify(Variable value)
		{
			if (Validate(value))
			{
				_value = value;
				Refresh();
			}
		}

		private bool Validate(Variable value)
		{
			if (_definition != null && !_definition.IsValid(value))
			{
				var valid = _definition.Generate();
				schedule.Execute(() => this.SendChangeEvent(_value, valid)).StartingIn(0);
				return false;
			}

			return true;
		}

		private void CreateElements()
		{
			CreateEmpty();
			CreateBool();
			CreateInt();
			CreateFloat();
			CreateVector2Int();
			CreateVector3Int();
			CreateRectInt();
			CreateBoundsInt();
			CreateVector2();
			CreateVector3();
			CreateVector4();
			CreateQuaternion();
			CreateRect();
			CreateBounds();
			CreateColor();
			CreateEnum();
			CreateString();
			CreateList();
			CreateDictionary();
			CreateAsset();
			CreateObject();
			CreateReset();
		}

		public void Refresh()
		{
			switch (_value.Type)
			{
				case VariableType.Empty: RefreshEmpty(); break;
				case VariableType.Bool: RefreshBool(); break;
				case VariableType.Int: RefreshInt(); break;
				case VariableType.Float: RefreshFloat(); break;
				case VariableType.Vector2Int: RefreshVector2Int(); break;
				case VariableType.Vector3Int: RefreshVector3Int(); break;
				case VariableType.RectInt: RefreshRectInt(); break;
				case VariableType.BoundsInt: RefreshBoundsInt(); break;
				case VariableType.Vector2: RefreshVector2(); break;
				case VariableType.Vector3: RefreshVector3(); break;
				case VariableType.Vector4: RefreshVector4(); break;
				case VariableType.Quaternion: RefreshQuaternion(); break;
				case VariableType.Rect: RefreshRect(); break;
				case VariableType.Bounds: RefreshBounds(); break;
				case VariableType.Color: RefreshColor(); break;
				case VariableType.Enum: RefreshEnum(); break;
				case VariableType.String: RefreshString(); break;
				case VariableType.List: RefreshList(); break;
				case VariableType.Dictionary: RefreshDictionary(); break;
				case VariableType.Asset: RefreshAsset(); break;
				case VariableType.Object: RefreshObject(); break;
			}

			_resetButton.SetDisplayed(_value.Type != VariableType.Empty && _definition == null);
			_emptyField.SetDisplayed(_value.Type == VariableType.Empty);
			_boolToggle.SetDisplayed(_value.Type == VariableType.Bool);
			_intContainer.SetDisplayed(_value.Type == VariableType.Int);
			_floatContainer.SetDisplayed(_value.Type == VariableType.Float);
			_vector2IntField.SetDisplayed(_value.Type == VariableType.Vector2Int);
			_vector3IntField.SetDisplayed(_value.Type == VariableType.Vector3Int);
			_rectIntField.SetDisplayed(_value.Type == VariableType.RectInt);
			_boundsIntField.SetDisplayed(_value.Type == VariableType.BoundsInt);
			_vector2Field.SetDisplayed(_value.Type == VariableType.Vector2);
			_vector3Field.SetDisplayed(_value.Type == VariableType.Vector3);
			_vector4Field.SetDisplayed(_value.Type == VariableType.Vector4);
			_quaternionField.SetDisplayed(_value.Type == VariableType.Quaternion);
			_rectField.SetDisplayed(_value.Type == VariableType.Rect);
			_boundsField.SetDisplayed(_value.Type == VariableType.Bounds);
			_colorField.SetDisplayed(_value.Type == VariableType.Color);
			_enumField.SetDisplayed(_value.Type == VariableType.Enum);
			_stringContainer.SetDisplayed(_value.Type == VariableType.String);
			_listField.SetDisplayed(_value.Type == VariableType.List);
			_dictionaryField.SetDisplayed(_value.Type == VariableType.Dictionary);
			_assetField.SetDisplayed(_value.Type == VariableType.Asset);
			_objectField.SetDisplayed(_value.Type == VariableType.Object);
		}

		#region Empty

		private void CreateEmpty()
		{
			_emptyField = new EnumField(VariableType.Empty);
			_emptyField.AddToClassList(EmptyUssClassName);
			_emptyField.RegisterValueChangedCallback(evt =>
			{
				var type = (VariableType)evt.newValue;
				var value = Variable.Create(type);

				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_emptyField);

		}

		private void RefreshEmpty()
		{
			_emptyField.SetValueWithoutNotify(VariableType.Empty);
			_emptyField.SetEnabled(_definition == null);
		}

		private void CreateReset()
		{
			_resetButton = new IconButton(() => this.SendChangeEvent(_value, Variable.Empty))
			{
				image = Icon.Refresh.Texture
			};

			_resetButton.AddToClassList(ResetUssClassName);

			Add(_resetButton);
		}

		#endregion

		#region Bool

		private void CreateBool()
		{
			_boolToggle = new Toggle();
			_boolToggle.AddToClassList(BoolUssClassName);
			_boolToggle.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Bool(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_boolToggle);
		}

		private void RefreshBool()
		{
			_boolToggle.SetValueWithoutNotify(_value.AsBool);
		}

		#endregion

		#region Int

		private void CreateInt()
		{
			_intContainer = new VisualElement();
			_intContainer.AddToClassList(NumberUssClassName);

			_intSlider = new SliderInt();
			_intSlider.AddToClassList(NumberSliderUssClassName);
			_intSlider.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Int(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			_intField = new IntegerField();
			_intField.AddToClassList(NumberFieldUssClassName);
			_intField.RegisterValueChangedCallback(evt =>
			{
				if (_definition != null && _definition.Constraint is IntConstraint constraint)
				{
					var minimum = constraint.HasMinimum ? constraint.Minimum : int.MinValue;
					var maximum = constraint.HasMaximum ? constraint.Maximum : int.MaxValue;
					var clamped = Mathf.Clamp(evt.newValue, minimum, maximum);
					var value =  Variable.Int(clamped);

					this.SendChangeEvent(_value, value);
				}
				else
				{
					var value = Variable.Int(evt.newValue);
					this.SendChangeEvent(_value, value);
				}

				evt.StopImmediatePropagation();
			});

			_intContainer.Add(_intSlider);
			_intContainer.Add(_intField);

			Add(_intContainer);
		}

		private void RefreshInt()
		{
			if (_definition != null && _definition.Constraint is IntConstraint constraint)
			{
				_intContainer.EnableInClassList(NumberHasRangeUssClassName, constraint.HasMinimum && constraint.HasMaximum);
				_intSlider.SetValueWithoutNotify(_value.AsInt);
				_intSlider.lowValue = constraint.HasMinimum ? constraint.Minimum : IntConstraint.DefaultMinimum;
				_intSlider.highValue = constraint.HasMaximum ? constraint.Maximum : IntConstraint.DefaultMaximum;
			}
			else
			{
				_intSlider.SetDisplayed(false);
			}

			_intField.SetValueWithoutNotify(_value.AsInt);
		}

		#endregion

		#region Float

		private void CreateFloat()
		{
			_floatContainer = new VisualElement();
			_floatContainer.AddToClassList(NumberUssClassName);

			_floatSlider = new Slider();
			_floatSlider.AddToClassList(NumberSliderUssClassName);
			_floatSlider.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Float(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			_floatField = new FloatField();
			_floatField.AddToClassList(NumberFieldUssClassName);
			_floatField.RegisterValueChangedCallback(evt =>
			{
				if (_definition != null && _definition.Constraint is FloatConstraint constraint)
				{
					var minimum = constraint.HasMinimum ? constraint.Minimum : float.MinValue;
					var maximum = constraint.HasMaximum ? constraint.Maximum : float.MaxValue;
					var clamped = Mathf.Clamp(evt.newValue, minimum, maximum);
					var value = Variable.Float(clamped);

					this.SendChangeEvent(_value, value);
				}
				else
				{
					var value = Variable.Float(evt.newValue);
					this.SendChangeEvent(_value, value);
				}

				evt.StopImmediatePropagation();
			});

			_floatContainer.Add(_floatSlider);
			_floatContainer.Add(_floatField);

			Add(_floatContainer);
		}

		private void RefreshFloat()
		{
			if (_definition != null && _definition.Constraint is FloatConstraint constraint)
			{
				_floatContainer.EnableInClassList(NumberHasRangeUssClassName, constraint.HasMinimum && constraint.HasMaximum);
				_floatSlider.SetValueWithoutNotify(_value.AsFloat);
				_floatSlider.lowValue = constraint.HasMinimum ? constraint.Minimum : FloatConstraint.DefaultMinimum;
				_floatSlider.highValue = constraint.HasMaximum ? constraint.Maximum : FloatConstraint.DefaultMaximum;
			}
			else
			{
				_floatSlider.SetDisplayed(false);
			}

			_floatField.SetValueWithoutNotify(_value.AsFloat);
		}

		#endregion

		#region Vector2Int

		private void CreateVector2Int()
		{
			_vector2IntField = new Vector2IntField();
			_vector2IntField.AddToClassList(FieldUssClassName);
			_vector2IntField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Vector2Int(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_vector2IntField);
		}

		private void RefreshVector2Int()
		{
			_vector2IntField.SetValueWithoutNotify(_value.AsVector2Int);
		}

		#endregion

		#region Vector3Int

		private void CreateVector3Int()
		{
			_vector3IntField = new Vector3IntField();
			_vector3IntField.AddToClassList(FieldUssClassName);
			_vector3IntField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Vector3Int(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_vector3IntField);
		}

		private void RefreshVector3Int()
		{
			_vector3IntField.SetValueWithoutNotify(_value.AsVector3Int);
		}

		#endregion

		#region RectInt

		private void CreateRectInt()
		{
			_rectIntField = new RectIntField();
			_rectIntField.AddToClassList(FieldUssClassName);
			_rectIntField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.RectInt(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_rectIntField);
		}

		private void RefreshRectInt()
		{
			_rectIntField.SetValueWithoutNotify(_value.AsRectInt);
		}

		#endregion

		#region BoundsInt

		private void CreateBoundsInt()
		{
			_boundsIntField = new BoundsIntField();
			_boundsIntField.AddToClassList(FieldUssClassName);
			_boundsIntField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.BoundsInt(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_boundsIntField);
		}

		private void RefreshBoundsInt()
		{
			_boundsIntField.SetValueWithoutNotify(_value.AsBoundsInt);
		}

		#endregion

		#region Vector2

		private void CreateVector2()
		{
			_vector2Field = new Vector2Field();
			_vector2Field.AddToClassList(FieldUssClassName);
			_vector2Field.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Vector2(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_vector2Field);
		}

		private void RefreshVector2()
		{
			_vector2Field.SetValueWithoutNotify(_value.AsVector2);
		}

		#endregion

		#region Vector3

		private void CreateVector3()
		{
			_vector3Field = new Vector3Field();
			_vector3Field.AddToClassList(FieldUssClassName);
			_vector3Field.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Vector3(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_vector3Field);
		}

		private void RefreshVector3()
		{
			_vector3Field.SetValueWithoutNotify(_value.AsVector3);
		}

		#endregion

		#region Vector4

		private void CreateVector4()
		{
			_vector4Field = new Vector4Field();
			_vector4Field.AddToClassList(FieldUssClassName);
			_vector4Field.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Vector4(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_vector4Field);
		}

		private void RefreshVector4()
		{
			_vector4Field.SetValueWithoutNotify(_value.AsVector4);
		}

		#endregion

		#region Quaternion

		private void CreateQuaternion()
		{
			_quaternionField = new EulerField();
			_quaternionField.RegisterCallback<ChangeEvent<Quaternion>>(evt =>
			{
				var value = Variable.Quaternion(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_quaternionField);
		}

		private void RefreshQuaternion()
		{
			_quaternionField.SetValueWithoutNotify(_value.AsQuaternion);
		}

		#endregion

		#region Rect

		private void CreateRect()
		{
			_rectField = new RectField();
			_rectField.AddToClassList(FieldUssClassName);
			_rectField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Rect(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_rectField);
		}

		private void RefreshRect()
		{
			_rectField.SetValueWithoutNotify(_value.AsRect);
		}

		#endregion

		#region Bounds

		private void CreateBounds()
		{
			_boundsField = new BoundsField();
			_boundsField.AddToClassList(FieldUssClassName);
			_boundsField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Bounds(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_boundsField);
		}

		private void RefreshBounds()
		{
			_boundsField.SetValueWithoutNotify(_value.AsBounds);
		}

		#endregion

		#region Color

		private void CreateColor()
		{
			_colorField = new ColorField();
			_colorField.AddToClassList(FieldUssClassName);
			_colorField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Color(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_colorField);
		}

		private void RefreshColor()
		{
			_colorField.SetValueWithoutNotify(_value.AsColor);
		}

		#endregion

		#region Enum

		private void CreateEnum()
		{
			_enumField = new EnumField();
			_enumField.AddToClassList(FieldUssClassName);
			_enumField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.Enum(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_enumField);
		}

		private void RefreshEnum()
		{
			_enumField.Init(_value.AsEnum);
		}

		#endregion

		#region String

		private void CreateString()
		{
			_stringContainer = new VisualElement();
			_stringContainer.AddToClassList(FieldUssClassName);

			_stringField = new TextField { isDelayed = true };
			_stringField.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.String(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			_stringPopup = new PopupStringField();
			_stringPopup.RegisterValueChangedCallback(evt =>
			{
				var value = Variable.String(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			_stringContainer.Add(_stringField);
			_stringContainer.Add(_stringPopup);

			Add(_stringContainer);
		}

		private void RefreshString()
		{
			if (_definition != null && _definition.Constraint is StringConstraint constraint && constraint.Values.Count > 0)
			{
				_stringPopup.SetValues(constraint.Values);
				_stringPopup.SetValueWithoutNotify(_value.AsString);
				_stringField.SetDisplayed(false);
				_stringPopup.SetDisplayed(true);
				_stringContainer.Add(_stringPopup);
			}
			else
			{
				_stringPopup.SetDisplayed(false);
				_stringField.SetDisplayed(true);
				_stringField.SetValueWithoutNotify(_value.AsString);
			}
		}

		#endregion

		#region Asset

		private void CreateAsset()
		{
			_assetField = new ObjectField { objectType = typeof(ScriptableObject) };
			//_assetField = new ObjectPickerField(typeof(ScriptableObject));
			_assetField.AddToClassList(FieldUssClassName);
			_assetField.RegisterCallback<ChangeEvent<Object>>(evt =>
			{
				var value = Variable.Asset(new AssetReference(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(evt.newValue))));
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_assetField);
		}

		private void RefreshAsset()
		{
			_assetField.SetValueWithoutNotify(_value.GetObject<Object>());
		}

		#endregion

		#region Object

		private void CreateObject()
		{
			var objectType = (_definition?.Constraint as ObjectConstraint)?.ObjectType ?? typeof(Object);
			_objectField = new ObjectField { objectType = objectType };
			_objectField.AddToClassList(FieldUssClassName);
			_objectField.RegisterCallback<ChangeEvent<Object>>(evt =>
			{
				var value = Variable.Object(evt.newValue);
				this.SendChangeEvent(_value, value);
				evt.StopImmediatePropagation();
			});

			Add(_objectField);
		}

		private void RefreshObject()
		{
			if (_definition != null && _definition.Constraint is ObjectConstraint constraint)
				_objectField.objectType = constraint.ObjectType ?? typeof(Object);

			_objectField.SetValueWithoutNotify(_value.GetObject<Object>());
		}

		#endregion

		#region List

		private void CreateList()
		{
			_listProxy = new VariableListProxy();
			_listField = new ListField
			{
				Label = "Variables",
				Tooltip = "The variables in this list",
				EmptyLabel = "This list has no variables",
				EmptyTooltip = "There are no variables in this list",
				AddTooltip = "Add a variable to this list",
				RemoveTooltip = "Remove this variable from the list",
				ReorderTooltip = "Move this variable in the list"
			};

			_listField.AddToClassList(ContainerUssClassName);
			_listField.SetProxy(_listProxy, null, false);

			_listField.RegisterCallback<ListField.ItemAddedEvent>(evt =>
			{
				this.SendChangeEvent(_value, _value);
				evt.StopImmediatePropagation();
			});

			_listField.RegisterCallback<ListField.ItemRemovedEvent>(evt =>
			{
				this.SendChangeEvent(_value, _value);
				evt.StopImmediatePropagation();
			});

			_listField.RegisterCallback<ListField.ItemReorderedEvent>(evt =>
			{
				this.SendChangeEvent(_value, _value);
				evt.StopImmediatePropagation();
			});

			Add(_listField);
		}

		private void RefreshList()
		{
			if (_definition != null && _definition.Constraint is ListConstraint constraint)
				_listProxy.Definition = new VariableDefinition(string.Empty, constraint.ItemConstraint);
			else
				_listProxy.Definition = null;

			_listProxy.Variables = _value.AsList;
			_listField.Rebuild();
		}

		private class VariableListProxy : IListProxy
		{
			public IVariableList Variables;
			public VariableDefinition Definition;

			public int Count => Variables?.VariableCount ?? 0;

			public bool CanAdd() => true;
			public bool CanAdd(Type type) => true;
			public bool CanRemove(int index) => true;

			public VariableListProxy()
			{
			}

			public VisualElement CreateElement(int index)
			{
				var value = Variables.GetVariable(index);
				var control = new VariableControl(Definition);
				control.SetValueWithoutNotify(value);
				control.RegisterCallback<ChangeEvent<Variable>>(evt => Variables.SetVariable(index, evt.newValue));

				return control;
			}

			public bool AddItem(Type type)
			{
				var value = Definition != null ? Definition.Generate() : Variable.Empty;
				Variables.AddVariable(value);
				return true;
			}

			public void RemoveItem(int index)
			{
				Variables.RemoveVariable(index);
			}

			public void ReorderItem(int from, int to)
			{
				var variable = Variables.GetVariable(from);
				Variables.RemoveVariable(from);
				Variables.InsertVariable(to, variable);
			}
		}

		#endregion

		#region Dictionary

		private void CreateDictionary()
		{
			_dictionaryProxy = new VariableDictionaryProxy();
			_dictionaryField = new DictionaryField
			{
				AllowReorder = false,
				Label = "Variables",
				Tooltip = "The Variables in this dictionary",
				EmptyLabel = "This Dictionary has no Variables",
				EmptyTooltip = "There are no Variables in this dictionary",
				AddPlaceholder = "New Variable",
				AddTooltip = "Add a Variable to this dictionary",
				RemoveTooltip = "Remove this Variable",
				ReorderTooltip = "Move this Variable"
			};

			_dictionaryField.AddToClassList(ContainerUssClassName);
			_dictionaryField.SetProxy(_dictionaryProxy, null, false);

			_dictionaryField.RegisterCallback<DictionaryField.ItemAddedEvent>(evt =>
			{
				this.SendChangeEvent(_value, _value);
				evt.StopImmediatePropagation();
			});

			_dictionaryField.RegisterCallback<DictionaryField.ItemRemovedEvent>(evt =>
			{
				this.SendChangeEvent(_value, _value);
				evt.StopImmediatePropagation();
			});

			Add(_dictionaryField);
		}

		private void RefreshDictionary()
		{
			if (_definition != null && _definition.Constraint is DictionaryConstraint constraint)
				_dictionaryProxy.Schema = constraint.Schema;
			else
				_dictionaryProxy.Schema = null;

			_dictionaryProxy.Variables = _value.AsDictionary;
			_dictionaryField.Rebuild();
		}

		private class VariableDictionaryProxy : IDictionaryProxy
		{
			public IVariableDictionary Variables;
			public VariableSchema Schema;

			public int Count => Variables?.VariableNames.Count ?? 0;
			public bool IsReorderable => false;

			public bool CanAdd(string key) => Variables != null && !Variables.VariableNames.Contains(key);
			public bool CanAdd(Type type) => true;
			public bool CanRemove(int index, string key) => true;
			public void RemoveItem(int index, string key) => Variables.RemoveVariable(key);
			public void ReorderItem(int from, int to) { }

			public VariableDictionaryProxy()
			{
			}

			public string GetKey(int index)
			{
				return GetName(index);
			}

			public VisualElement CreateElement(int index, string key)
			{
				var container = new VisualElement();
				var name = GetName(index);
				var value = Variables.GetVariable(name);
				var definition = GetDefinition(name);

				var label = new Label(name);
				label.AddToClassList(DictionaryLabelUssClassName);

				var control = new VariableControl(definition);
				control.SetValueWithoutNotify(value);

				container.Add(label);
				container.Add(control);

				return container;
			}

			public bool AddItem(string key, Type type)
			{
				var definition = GetDefinition(key);
				var value = definition != null ? definition.Generate() : Variable.Empty;
				Variables.SetVariable(key, value);
				return true;
			}

			public void RemoveItem(int index)
			{
				var name = GetName(index);
				Variables.SetVariable(name, Variable.Empty);
			}

			private string GetName(int index)
			{
				return Variables.VariableNames.ElementAt(index);
			}

			private VariableDefinition GetDefinition(string name)
			{
				return Schema?.Definitions
					.Where(d => d.Name == name)
					.FirstOrDefault();
			}
		}

		#endregion
	}
}
