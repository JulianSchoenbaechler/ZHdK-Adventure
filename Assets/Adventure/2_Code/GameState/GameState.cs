using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JulianSchoenbaechler.GameState
{
	public class GameState : MonoBehaviour
	{
		#region Variables

		private static IGameState _active = null;						// Active game state object
		protected static Dictionary<string, IGameState> _allStates;		// All initialized game states
		[SerializeField] protected string _defaultState = null;			// The default game state ('null' for none)

		#endregion

		#region Properties

		/// <summary>
		/// The instance in the scene.
		/// </summary>
		public static GameObject instance
		{
			get;
			protected set;
		}

		/// <summary>
		/// The name of the active game state.
		/// </summary>
		public static string active
		{
			get
			{
				return GameState._active.name;
			}

			set
			{
				if(_allStates.ContainsKey(value))
				{
					if(GameState._active != null)
						GameState._active.ResetState();

					GameState._active = _allStates[value];
					GameState._active.Start();
				}
				else
				{
					Debug.LogWarning("[GameState] GameState '" + value + "' does not exist. Cannot switch to new state.");
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Prefills the game states dictionary.
		/// </summary>
		private void PrefillStateDictionary()
		{
			// Reflection to gather all game states
			var type = typeof(IGameState);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsClass && type.IsAssignableFrom(x))
				.Where(x => Attribute.IsDefined(x, typeof(CustomGameState)));

			_allStates = new Dictionary<string, IGameState>(types.Count<Type>());

			IGameState tempInstance;

			foreach(var t in types)
			{
				tempInstance = (IGameState)Activator.CreateInstance(t);
				tempInstance.name = t.ToString();
				_allStates.Add(t.ToString(), tempInstance);
			}
		}

		#endregion

		#region Unity Behaviour

		/// <summary>
		/// Awake method.
		/// </summary>
		private void Awake()
		{
			PrefillStateDictionary();

			if(GameState.instance == null)
			{
				instance = gameObject;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		/// <summary>
		/// Initialization.
		/// </summary>
		private void Start()
		{
			if((_defaultState != null) && (_defaultState.Length > 0))
				GameState.active = _defaultState;
		}

		/// <summary>
		/// Every frame.
		/// </summary>
		private void Update()
		{
			if(GameState._active != null)
				GameState._active.Update();
		}

		/// <summary>
		/// After every frame.
		/// </summary>
		private void LateUpdate()
		{
			if(GameState._active != null)
				GameState._active.LateUpdate();
		}

		/// <summary>
		/// Physics.
		/// </summary>
		private void FixedUpdate()
		{
			if(GameState._active != null)
				GameState._active.FixedUpdate();
		}

		#endregion
	}
}
