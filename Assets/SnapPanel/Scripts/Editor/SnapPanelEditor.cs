using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace JulianSchoenbaechler.SnapPanel
{
	[CustomEditor(typeof(SnapPanel))]
	public class SnapPanelEditor : Editor
	{
		#region Variables

		private SerializedProperty _panelsProp;
		private SerializedProperty _cameraPositionsProp;
		private SerializedProperty _focusProp;
		private SerializedProperty _chooseRandomProp;
		private SerializedProperty _snapDelayProp;
		private SerializedProperty _panelLifetimeProp;
		private SerializedProperty _panelTypeProp;

		#endregion

		#region Unity Behaviour

		/// <summary>
		/// On enable.
		/// </summary>
		private void OnEnable()
		{
			// Get serialized references
			_panelsProp = serializedObject.FindProperty("_panels");
			_cameraPositionsProp = serializedObject.FindProperty("_cameraPositions");
			_focusProp = serializedObject.FindProperty("_focus");
			_chooseRandomProp = serializedObject.FindProperty("_chooseRandom");
			_snapDelayProp = serializedObject.FindProperty("_snapDelay");
			_panelLifetimeProp = serializedObject.FindProperty("_panelLifetime");
			_panelTypeProp = serializedObject.FindProperty("panelType");

			// Register playmode state change event
			Playmode.playmodeStateChanged += OnPlaymodeStateChange;
		}

		/// <summary>
		/// Inspector GUI event...
		/// </summary>
		public override void OnInspectorGUI()
		{
			// Update serialized property values
			serializedObject.Update();

			// Draw properties
			EditorGUILayout.PropertyField(_panelsProp, new GUIContent("Panels", "An array of objects used as comic panels. The gameobject or one of its children must contain a 'RawImage' component."), true);
			EditorGUILayout.PropertyField(_cameraPositionsProp, new GUIContent("Camera Positions", "An array of objects used as transform / location indicator for the camera positions. The size of this array must be equal to the size of the 'Panels' array unless 'Random Positions' is checked."), true);
			EditorGUILayout.PropertyField(_focusProp, new GUIContent("Focus Point", "The 'look at' indicator for the cameras."));
			EditorGUILayout.PropertyField(_chooseRandomProp, new GUIContent("Choose Random", "If checked, the camera positions will be randomized."));
			EditorGUILayout.PropertyField(_snapDelayProp, new GUIContent("Snap Delay", "The time delay between rendering the panels in seconds."));
			EditorGUILayout.PropertyField(_panelLifetimeProp, new GUIContent("Lifetime", "The lifetime of the panels in seconds."));
			EditorGUILayout.PropertyField(_panelTypeProp);

			// Apply property changes
			serializedObject.ApplyModifiedProperties();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Playmode state change event listener.
		/// </summary>
		/// <param name="playmode">Playmode state.</param>
		private void OnPlaymodeStateChange(Playmode.PlaymodeState playmode)
		{
			// Before entering playmode
			if(playmode == Playmode.PlaymodeState.AboutToPlay)
			{
				try
				{
					// Check if array size is correct
					if(!_chooseRandomProp.boolValue && (_panelsProp.arraySize != _cameraPositionsProp.arraySize))
					{
						EditorApplication.isPlaying = false;
						Debug.LogError("[SnapPanel] Array count error! Index of arrays 'Panels' and 'Camera Positions' must be equal.");
					}
				}
				catch(NullReferenceException)
				{
					// No exception handling
				}
			}
		}

		#endregion
	}
}
