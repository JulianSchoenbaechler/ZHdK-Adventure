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
	}
}
