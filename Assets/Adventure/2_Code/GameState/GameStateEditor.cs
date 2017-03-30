#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace JulianSchoenbaechler.GameState
{
	[CustomEditor(typeof(GameState))]
	public class GameStateEditor : Editor
	{
		private SerializedProperty _allStatesProp;


		/// <summary>
		/// On enable.
		/// </summary>
		private void OnEnable()
		{
			// Get serialized references
			_allStatesProp = serializedObject.FindProperty("_defaultState");

			StatesArray();
		}

		/// <summary>
		/// Inspector GUI event...
		/// </summary>
		public override void OnInspectorGUI()
		{
			string[] stateOptions = StatesArray();		// List of all states
			int selectedState = 0;						// current selected state

			// Get index of selected state
			if((_allStatesProp.stringValue != null) && (_allStatesProp.stringValue.Length > 0))
				selectedState = Array.IndexOf(stateOptions, _allStatesProp.stringValue);

			// Update serialized property values
			serializedObject.Update();

			// Draw properties
			selectedState = EditorGUILayout.Popup("Default Game State", selectedState, stateOptions);
			_allStatesProp.stringValue = stateOptions[selectedState] == "None" ? null : stateOptions[selectedState];

			// Apply property changes
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Creates an array of all possible state names
		/// </summary>
		private string[] StatesArray()
		{
			string[] stateNames;
			int i = 1;

			// Reflection to gather all game states
			var type = typeof(IGameState);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsClass && type.IsAssignableFrom(x))
				.Where(x => Attribute.IsDefined(x, typeof(CustomGameState)));

			stateNames = new string[types.Count<Type>() + 1];
			stateNames[0] = "None";

			foreach(var t in types)
			{
				stateNames[i] = t.ToString();
				i++;
			}

			return stateNames;
		}
	}
}
#endif
