using System.Collections;
using System.Collections.Generic;

namespace Adventure.Inventory
{
	public class Inventory
	{
		protected int _capacity;
		protected Dictionary<string, Item> _inventory;


		public Inventory()
		{
			_capacity = 0;
			_inventory = new Dictionary<string, Item>();
		}

		public Inventory(int capacity)
		{
			_capacity = capacity;
			_inventory = new Dictionary<string, Item>(capacity);
		}

		public void Add(Item item)
		{
			if(item.ID != null)
			{
				if(!ExistsInInventory(item.ID))
				{
					_inventory.Add(item.ID, item);

					if(_capacity < _inventory.Count)
						_capacity = _inventory.Count;
				}
			}
		}

		public void Remove(string id)
		{
			if(ExistsInInventory(id))
			{
				_inventory.Remove(id);
			}
		}

		public void Duplicate(string id)
		{
			if(ExistsInInventory(id))
			{
				if(_inventory[id].IsStackable)
				{
					Item clone = _inventory[id].Clone() as Item;
					Add(clone);
				}
			}
		}

		public bool ExistsInInventory(string id)
		{
			return _inventory.ContainsKey(id);
		}

		public void EmptyInventory()
		{
			_inventory.Clear();
		}
	}
}
