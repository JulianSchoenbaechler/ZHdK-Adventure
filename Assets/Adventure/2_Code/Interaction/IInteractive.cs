using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Interaction
{
	public enum SpecialAction
	{
		None,
		LoadScene,
		ChangeState,
		DisableObject,
		EnableObject
	};

	public interface IInteractive
	{
		event Action InteractionEvent;
		bool Active { get; set; }
	}
}
