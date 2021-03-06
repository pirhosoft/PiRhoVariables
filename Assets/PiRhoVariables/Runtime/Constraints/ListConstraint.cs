﻿using System;
using UnityEngine;

namespace PiRhoSoft.Variables
{
	[Serializable]
	public class ListConstraint : VariableConstraint
	{
		public override VariableType Type => VariableType.List;

		public VariableType ItemType { get => _itemType; set => SetType(value); }
		public VariableConstraint ItemConstraint { get => _itemConstraint; set => SetConstraint(value); }

		[SerializeField] private VariableType _itemType;
		[SerializeReference] private VariableConstraint _itemConstraint;

		public ListConstraint()
		{
			ItemType = VariableType.Empty;
		}

		public ListConstraint(VariableType type)
		{
			ItemType = type;
		}

		public ListConstraint(VariableConstraint itemConstraint)
		{
			ItemConstraint = itemConstraint;
		}

		public override string ToString()
		{
			if (ItemConstraint == null)
				return Type.ToString();
			else
				return string.Format($"{ItemType}({ItemConstraint})");
		}

		public override Variable Generate()
		{
			return Variable.List(new VariableList());
		}

		public override bool IsValid(Variable variable)
		{
			if (variable.TryGetList(out var list))
			{
				for (var i = 0; i < list.VariableCount; i++)
				{
					var item = list.GetVariable(i);

					if (ItemConstraint != null)
					{
						if (!ItemConstraint.IsValid(item))
							return false;
					}
					else if (ItemType != VariableType.Empty)
					{
						if (!item.Is(ItemType))
							return false;
					}
				}

				return true;
			}

			return false;
		}

		private void SetType(VariableType type)
		{
			_itemType = type;
			_itemConstraint = Create(type);
		}

		private void SetConstraint(VariableConstraint constraint)
		{
			_itemType = constraint?.Type ?? VariableType.Empty;
			_itemConstraint = constraint;
		}
	}
}
