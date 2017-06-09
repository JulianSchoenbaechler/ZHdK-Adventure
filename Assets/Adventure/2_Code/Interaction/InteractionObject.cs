using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Adventure.Inventory;
using Adventure.UI;
using Adventure.Character;
using JulianSchoenbaechler.GameState;

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

			public void Run()
			{
				switch(_actionType)
				{
					case ActionType.LoadScene:
						SceneManager.LoadScene(_stringParameter);
						break;

					case ActionType.ChangeState:
						GameState.active = _stringParameter;
						break;

					case ActionType.DisableObject:
						_objectParameter.SetActive(false);
						break;

					case ActionType.EnableObject:
						_objectParameter.SetActive(true);
						break;

					case ActionType.ToggleObject:
						_objectParameter.SetActive(_objectParameter.activeInHierarchy);
						break;

					case ActionType.DestroyObject:
						Destroy(_objectParameter);
						break;

					case ActionType.InstantiateObject:
						Instantiate(
							_objectParameter,
							_positionParameter != null ? _positionParameter.position : Vector3.zero,
							Quaternion.identity,
							_objectParameter.transform.root
						);
						break;

					case ActionType.AddToInventory:
						break;

					case ActionType.RemoveFromInventory:
						break;

					default:
						break;
				}
			}
		}

		/// <summary>
		/// Occurs when player interacts with this object.
		/// </summary>
		public event Action InteractionEvent;


		[SerializeField] private bool _active = true;
		[SerializeField] private bool _onMouseClick = true;
		[SerializeField] private int _mouseButton = 0;
		[SerializeField] private bool _onKeyDown = false;
		[SerializeField] private KeyCode _interactionKey;
		[SerializeField] protected float _interactionDistance = 1f;
		[SerializeField] private InteractionPiece[] _interactions;

		protected Transform _player;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Adventure.Interaction.InteractionObject"/> is active.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}


		/// <summary>
		/// Initialization.
		/// </summary>
		private void Start()
		{
			_player = GameObject.FindGameObjectWithTag("Player").transform;
		}

		/// <summary>
		/// Every frame.
		/// </summary>
		protected void Update()
		{
			if(_active)
			{
				if(Vector3.Distance(transform.position, _player.position) <= _interactionDistance)
				{
					if(_onKeyDown && Input.GetKeyDown(_interactionKey))
					{
						OnInteraction();
					}

					// Edit only for Adventure -> hack
					if(GetComponent<WorldspaceImage>() != null)
					{
						if(GetComponent<SheepFollow>() == null)
						{
							if(!GetComponent<WorldspaceImage>().enabled)
								GetComponent<WorldspaceImage>().enabled = true;
						}
					}
				}
				else
				{
					// Edit only for Adventure -> hack
					if(GetComponent<WorldspaceImage>() != null)
					{
						if(GetComponent<SheepFollow>() == null)
						{
							if(GetComponent<WorldspaceImage>().enabled)
								GetComponent<WorldspaceImage>().enabled = false;
						}
					}
				}
			}
		}

		/// <summary>
		/// Every frame while mouse over interaction object.
		/// </summary>
		protected void OnMouseOver()
		{
			if(_active)
			{
				if(_onMouseClick && Input.GetMouseButtonDown(_mouseButton))
				{
					if(Vector3.Distance(transform.position, _player.position) <= _interactionDistance)
					{
						OnInteraction();
					}
				}
			}
		}

		/// <summary>
		/// Processes all interactions and raises the interaction event.
		/// </summary>
		public virtual void OnInteraction()
		{
			// Run all interactions
			for(int i = 0; i < _interactions.Length; i++)
			{
				_interactions[i].Run();
			}

			// Raise interaction event
			if(InteractionEvent != null)
				InteractionEvent();
		}

	}
}
