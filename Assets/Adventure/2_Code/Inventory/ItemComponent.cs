using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Inventory
{
	public class ItemComponent : MonoBehaviour
	{
		[SerializeField] private string _id = "id0";
		[SerializeField] private string _name = "noname";
		[SerializeField, TextArea] private string _description = "";
		[SerializeField] private bool _isStackable = false;
		[SerializeField] private Texture2D _icon;

		public Item Item { get; protected set; }


		protected void Awake()
		{
			_id = _id.Length > 0 ? _id : "id0";
			this.Item = new Item(_id, _name, _description, _isStackable);
			this.Item.Icon = _icon;
		}


		public static bool operator ==(ItemComponent itemA, ItemComponent itemB)
		{
			return String.Equals(itemA.Item.ID, itemB.Item.ID);
		}

		public static bool operator ==(ItemComponent itemA, Item itemB)
		{
			return String.Equals(itemA.Item.ID, itemB.ID);
		}

		public static bool operator !=(ItemComponent itemA, ItemComponent itemB)
		{
			return !String.Equals(itemA.Item.ID, itemB.Item.ID);
		}

		public static bool operator !=(ItemComponent itemA, Item itemB)
		{
			return !String.Equals(itemA.Item.ID, itemB.ID);
		}

		public static implicit operator Item(ItemComponent item)
		{
			return item.Item;
		}

		public override bool Equals(object other)
		{
			if(other is ItemComponent)
			{
				try
				{
					return String.Equals(((ItemComponent)other).Item.ID, _id);
				}
				catch(InvalidCastException e)
				{
					Debug.LogException(e);
					return false;
				}
			}

			return false;
		}

		public override int GetHashCode()
		{
			return (int)(_id[0] ^ _id[_id.Length - 1]);
		}
	}
}
