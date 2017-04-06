using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Inventory;

namespace Adventure.Interaction
{
	public class InteractionObject : MonoBehaviour, IInteractive
	{
		public event Action InteractionEvent;

		public bool Active { get; set; }

		[SerializeField] protected bool _active = true;
		[SerializeField] protected ActionType _specialAction = ActionType.None;
		[SerializeField, HideInInspector] private GameObject _objectParameter;
		[SerializeField, HideInInspector] private string _stringParameter;


		void Awake()
		{
			Active = _active;
		}

		// Use this for initialization
		void Start()
		{
			Item gaga = new Item("test");
			Debug.Log(gaga.ID);
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
