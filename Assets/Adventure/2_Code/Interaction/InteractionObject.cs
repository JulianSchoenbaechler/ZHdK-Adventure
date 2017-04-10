using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Inventory;

namespace Adventure.Interaction
{
	public class InteractionObject : MonoBehaviour, IInteractive
	{
		[Serializable]
		private sealed class InteractionPiece
		{
			public Transform Position { get; set; }

			[SerializeField] private ActionType _actionType = ActionType.None;
			[SerializeField, HideInInspector] private GameObject _objectParameter;
			[SerializeField, HideInInspector] private string _stringParameter;
			[SerializeField, HideInInspector] private Transform _positionParameter;
			[SerializeField, HideInInspector] private bool _stateParameter;
		}


		public event Action InteractionEvent;

		[SerializeField] protected bool _active = true;
		[SerializeField] private InteractionPiece[] _interactions;

		public bool Active { get; set; }


		void Awake()
		{
			Active = _active;
		}

		// Use this for initialization
		void Start()
		{
			// Debug
			Item item = new Item("test");
			item.AddProperty("marcel", "dumm");
			GameObject gaga = item.GetProperty<GameObject>("marcell");
			Debug.Log(gaga);
			//ItemComponent ic = GameObject.Find("Cube").GetComponent<ItemComponent>();
			//Item gaga = (Item)ic;
			//Debug.Log(ic.Equals(gaga));
		}

		// Update is called once per frame
		void Update()
		{

		}


		public virtual void OnInteraction()
		{
			// Raise interaction event
			if(InteractionEvent != null)
				InteractionEvent();
		}

	}
}
