using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Inventory
{
	public class Item : ICloneable
	{
		public string ID { get; protected set; }
		public string Name { get; protected set; }
		public string Description { get; protected set; }
		public bool IsStackable { get; protected set; }
		public Texture2D Icon { get; set; }

		protected Dictionary<string,  object> _itemProperties;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adventure.Inventory.Item"/> class.
		/// </summary>
		/// <param name="id">Item identifier.</param>
		public Item(string id)
		{
			ID = id.Length > 0 ? id : "id0";
			Name = "unnamed";
			Description = "";
			IsStackable = false;
			_itemProperties = new Dictionary<string, object>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adventure.Inventory.Item"/> class.
		/// </summary>
		/// <param name="id">Item identifier.</param>
		/// <param name="name">The item name.</param>
		public Item(string id, string name)
		{
			ID = id.Length > 0 ? id : "id0";
			Name = name;
			Description = "";
			IsStackable = false;
			_itemProperties = new Dictionary<string, object>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adventure.Inventory.Item"/> class.
		/// </summary>
		/// <param name="id">Item identifier.</param>
		/// <param name="name">The item name.</param>
		/// <param name="description">An item description.</param>
		public Item(string id, string name, string description)
		{
			ID = id.Length > 0 ? id : "id0";
			Name = name;
			Description = description;
			IsStackable = false;
			_itemProperties = new Dictionary<string, object>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adventure.Inventory.Item"/> class.
		/// </summary>
		/// <param name="id">Item identifier.</param>
		/// <param name="name">The item name.</param>
		/// <param name="stackable"><c>true</c> if item should be stackable.</param>
		public Item(string id, string name, bool stackable)
		{
			ID = id.Length > 0 ? id : "id0";
			Name = name;
			Description = "";
			IsStackable = stackable;
			_itemProperties = new Dictionary<string, object>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adventure.Inventory.Item"/> class.
		/// </summary>
		/// <param name="id">Item identifier.</param>
		/// <param name="name">The item name.</param>
		/// <param name="description">An item description.</param>
		/// <param name="stackable"><c>true</c> if item should be stackable.</param>
		public Item(string id, string name, string description, bool stackable)
		{
			ID = id.Length > 0 ? id : "id0";
			Name = name;
			Description = description;
			IsStackable = stackable;
			_itemProperties = new Dictionary<string, object>();
		}

		/// <summary>
		/// Adds a custom property to this item.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <param name="value">The property value.</param>
		public void AddProperty(string key, object value)
		{
			if((key == null) || (value == null))
			{
				Debug.LogError("[Inventory] Item.AddProperty: key and/or value can not be 'null'.");
				return;
			}

			if(!_itemProperties.ContainsKey(key))
			{
				_itemProperties[key] = value;
			}
		}

		/// <summary>
		/// Sets a custom property of this item. If the given key does not exist, a new property will be created.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <param name="value">The property value.</param>
		public void SetProperty(string key, object value)
		{
			if((key == null) || (value == null))
			{
				Debug.LogError("[Inventory] Item.SetProperty: key and/or value can not be 'null'.");
				return;
			}

			if(!_itemProperties.ContainsKey(key))
			{
				AddProperty(key, value);
			}
			else
			{
				_itemProperties[key] = value;
			}
		}

		/// <summary>
		/// Gets the value of a custom property.
		/// </summary>
		/// <returns>The property value.</returns>
		/// <param name="key">The property key.</param>
		public object GetProperty(string key)
		{
			if(key == null)
			{
				Debug.LogError("[Inventory] Item.GetProperty: key can not be 'null'.");
				return null;
			}

			if(_itemProperties.ContainsKey(key))
			{
				return _itemProperties[key];
			}

			return null;
		}

		/// <summary>
		/// Gets the value of a custom property.
		/// </summary>
		/// <returns>The property value.</returns>
		/// <param name="key">The property key.</param>
		/// <typeparam name="T">The type of the property to be returned.</typeparam>
		public T GetProperty<T>(string key)
		{
			if(key == null)
			{
				Debug.LogError("[Inventory] Item.GetProperty<T>: key can not be 'null'.");
				return default(T);
			}

			if(_itemProperties.ContainsKey(key))
			{
				try
				{
					return (T)_itemProperties[key];
				}
				catch(InvalidCastException e)
				{
					Debug.LogException(e);
				}
			}

			return default(T);
		}

		/// <summary>
		/// Getting all custom item properties as Dictionary object.
		/// </summary>
		/// <returns>Property collection.</returns>
		public Dictionary<string, object> GetAllProperties()
		{
			return _itemProperties;
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		/// <filterpriority>2</filterpriority>
		public object Clone()
		{
			Item clone = new Item(ID + "_clone", Name, Description, IsStackable);
			clone.Icon = Icon;

			foreach(KeyValuePair<string, object> entry in _itemProperties)
			{
				clone.AddProperty(entry.Key, entry.Value);
			}

			return clone;
		}
	}
}
