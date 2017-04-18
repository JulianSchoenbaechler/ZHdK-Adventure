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
		private SerializedProperty _interactionDistanceProp;
		private SerializedProperty _interactionsProp;
		private List<bool> _interactionsFoldout;


		/// <summary>
		/// On enable.
		/// </summary>
		private void OnEnable()
		{
			// Get serialized references
			_activeProp = serializedObject.FindProperty("_active");
			_interactionDistanceProp = serializedObject.FindProperty("_interactionDistance");
			_interactionsProp = serializedObject.FindProperty("_interactions");
			_interactionsFoldout = new List<bool>();
		}

		/// <summary>
		/// Inspector GUI event...
		/// </summary>
		public override void OnInspectorGUI()
		{
			// Update serialized property values
			serializedObject.Update();

			EditorGUILayout.PropertyField(_activeProp);
			EditorGUILayout.PropertyField(_interactionDistanceProp);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(_interactionsProp.FindPropertyRelative("Array.size"), new GUIContent("Numbers Of Interactions"));

			for(int i = 0; i < _interactionsProp.arraySize; i++)
			{
				
				DisplayInteraction(_interactionsProp.GetArrayElementAtIndex(i), i);
			}

			// Apply property changes
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Draw interaction options on inspector.
		/// </summary>
		/// <param name="interaction">Interactions array element as serialized property.</param>
		/// <param name="i">The interaction count.</param>
		private void DisplayInteraction(SerializedProperty interaction, int i)
		{
			SerializedProperty actionTypeProp = interaction.FindPropertyRelative("_actionType");
			SerializedProperty objectParameterProp = interaction.FindPropertyRelative("_objectParameter");
			SerializedProperty stringParameterProp = interaction.FindPropertyRelative("_stringParameter");
			SerializedProperty positionParameterProp = interaction.FindPropertyRelative("_positionParameter");

			while(_interactionsFoldout.Count <= i)
				_interactionsFoldout.Add(false);

			_interactionsFoldout[i] = EditorGUILayout.Foldout(_interactionsFoldout[i], "Interaction " + (i + 1).ToString("D"), true);

			if(_interactionsFoldout[i])
			{
				EditorGUI.indentLevel += 1;

				// Draw properties

				EditorGUILayout.PropertyField(actionTypeProp);

				switch(actionTypeProp.enumValueIndex)
				{
					case (int)ActionType.LoadScene:
						EditorGUILayout.PropertyField(stringParameterProp, new GUIContent("Scene Name"));
						break;

					case (int)ActionType.ChangeState:
						EditorGUILayout.PropertyField(stringParameterProp, new GUIContent("State Name"));
						break;

					case (int)ActionType.DisableObject:
					case (int)ActionType.EnableObject:
					case (int)ActionType.ToggleObject:
						EditorGUILayout.PropertyField(objectParameterProp, new GUIContent("Game Object"));
						break;

					case (int)ActionType.DestroyObject:
						EditorGUILayout.PropertyField(stringParameterProp, new GUIContent("Object By Tag"));
						EditorGUILayout.PropertyField(objectParameterProp, new GUIContent("Specific Game Object"));
						break;

					case (int)ActionType.InstantiateObject:
						EditorGUILayout.PropertyField(objectParameterProp, new GUIContent("Game Object"));
						EditorGUILayout.PropertyField(stringParameterProp, new GUIContent("Set Tag"));
						EditorGUILayout.PropertyField(positionParameterProp, new GUIContent("Position"));
						break;

					case (int)ActionType.AddToInventory:
						EditorGUILayout.PropertyField(objectParameterProp, new GUIContent("Item"));
						break;

					case (int)ActionType.RemoveFromInventory:
						EditorGUILayout.PropertyField(stringParameterProp, new GUIContent("Item ID"));
						break;

					default:
						break;
				}

				EditorGUI.indentLevel -= 1;
			}
		}
	}
}
#endif
