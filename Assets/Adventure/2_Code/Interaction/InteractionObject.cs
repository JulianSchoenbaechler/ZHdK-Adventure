using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Interaction
{
	public class InteractionObject : MonoBehaviour, IInteractive
	{
		public event Action InteractionEvent;

		public bool Active { get; set; }

		[SerializeField] protected bool _active = true;
		[SerializeField] protected SpecialAction _specialAction = SpecialAction.None;


		void Awake()
		{
			Active = _active;
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}


		public void OnInteraction()
		{
			// Raise interaction event
			if(InteractionEvent != null)
				InteractionEvent();
		}
	}
}
