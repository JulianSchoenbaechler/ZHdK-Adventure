using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Interaction
{
	public enum ActionType
	{
		None,
		LoadScene,
		ChangeState,
		DisableObject,
		EnableObject,
		ToggleObject,
		DestroyObject,
		InstantiateObject,
		AddToInventory,
		RemoveFromInventory
	};

	public interface IInteractive
	{
		event Action InteractionEvent;
		bool Active { get; set; }
	}
}
