using System.Collections;
using System.Collections.Generic;

public class Inventory
{
	protected int _capacity;
	protected Dictionary<string, Item> _inventory;

	public Inventory(int capacity)
	{
		_capacity = capacity;
	}
}
