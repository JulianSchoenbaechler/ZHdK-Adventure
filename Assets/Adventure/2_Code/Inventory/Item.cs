using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Inventory
{
	public class Item : MonoBehaviour, ICloneable
	{
		[SerializeField] private string _id = "id0";
		[SerializeField] private string _name = "noname";
		[SerializeField, TextArea] private string _description = "";
		[SerializeField] private bool _isStackable = false;
		[SerializeField] private Texture2D _icon;

		public string ID { get; protected set; }
		public string Name { get; protected set; }
		public string Description { get; protected set; }
		public bool IsStackable { get; protected set; }
		public Texture2D Icon { get; protected set; }


		public Item(string id)
		{
			ID = id;
			Name = "unnamed";
			Description = "";
			IsStackable = false;
		}

		public Item(string id, string name)
		{
			ID = id;
			Name = name;
			Description = "";
			IsStackable = false;
		}

		public Item(string id, string name, string description)
		{
			ID = id;
			Name = name;
			Description = description;
			IsStackable = false;
		}

		public Item(string id, string name, bool stackable)
		{
			ID = id;
			Name = name;
			Description = "";
			IsStackable = stackable;
		}

		public Item(string id, string name, string description, bool stackable)
		{
			ID = id;
			Name = name;
			Description = description;
			IsStackable = stackable;
		}

		public object Clone()
		{
			Item clone = new Item(ID + "_clone", Name, Description, IsStackable);

			return clone;
		}

		protected void Awake()
		{
			ID = _id.Length > 0 ? _id : "id";
			Name = _name;
			Description = _description;
			IsStackable = _isStackable;
			Icon = _icon;
		}
	}
}
