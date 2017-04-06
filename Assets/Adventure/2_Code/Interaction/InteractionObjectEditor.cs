#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Adventure.Interaction
{
	[CustomEditor(typeof(InteractionObject))]
	public class InteractionObjectEditor : Editor
	{
		private SerializedProperty _activeProp;
		private SerializedProperty _specialActionProp;
		private SerializedProperty _objectParameterProp;
		private SerializedProperty _stringParameterProp;


		/// <summary>
		/// On enable.
		/// </summary>
		private void OnEnable()
		{
			// Get serialized references
			_activeProp = serializedObject.FindProperty("_active");
			_specialActionProp = serializedObject.FindProperty("_specialAction");
			_objectParameterProp = serializedObject.FindProperty("_objectParameter");
			_stringParameterProp = serializedObject.FindProperty("_stringParameter");
		}

		/// <summary>
		/// Inspector GUI event...
		/// </summary>
		public override void OnInspectorGUI()
		{// Update serialized property values
			serializedObject.Update();

			// Draw properties
			EditorGUILayout.PropertyField(_activeProp);
			EditorGUILayout.PropertyField(_specialActionProp);

			switch(_specialActionProp.enumValueIndex)
			{
				case (int)ActionType.LoadScene:
					EditorGUILayout.PropertyField(_stringParameterProp, new GUIContent("Scene Name"));
					break;

				case (int)ActionType.ChangeState:
					EditorGUILayout.PropertyField(_stringParameterProp, new GUIContent("State Name"));
					break;

				case (int)ActionType.DisableObject:
				case (int)ActionType.EnableObject:
				case (int)ActionType.ToggleObject:
					EditorGUILayout.PropertyField(_objectParameterProp, new GUIContent("Game Object"));
					break;

				default:
					break;
			}

			// Apply property changes
			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif
