using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public string ID { get; protected set; }
	public string Name { get; protected set; }
	public string Description { get; protected set; }
	public Texture2D Icon { get; set; }
}
