#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Adventure.LevelManager
{
	[CustomEditor(typeof(LevelManager))]
	public class LevelManagerEditor : Editor
	{
		private SerializedProperty _playerObjectProp;
		private SerializedProperty _levelSavesProp;
		private List<bool> _levelSavesFoldout;

		/// <summary>
		/// On enable.
		/// </summary>
		private void OnEnable()
		{
			// Get serialized references
			_playerObjectProp = serializedObject.FindProperty("_playerObject");
			_levelSavesProp = serializedObject.FindProperty("_levelSaves");
			_levelSavesFoldout = new List<bool>();
		}

		/// <summary>
		/// Inspector GUI event...
		/// </summary>
		public override void OnInspectorGUI()
		{
			// Update serialized property values
			serializedObject.Update();

			EditorGUILayout.PropertyField(_playerObjectProp);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(_levelSavesProp.FindPropertyRelative("Array.size"), new GUIContent("Numbers Of Saves"));

			for(int i = 0; i < _levelSavesProp.arraySize; i++)
			{
				DisplaySave(_levelSavesProp.GetArrayElementAtIndex(i), i);
			}

			// Apply property changes
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Displaying the save options in inspector.
		/// </summary>
		/// <param name="save">LevelSaves array element as serialized property.</param>
		/// <param name="i">The level saves count.</param>
		private void DisplaySave(SerializedProperty save, int i)
		{
			SerializedProperty playerPositionProp = save.FindPropertyRelative("_playerPosition");
			SerializedProperty gameStateReferencesProp = save.FindPropertyRelative("_gameStateReferences");

			while(_levelSavesFoldout.Count <= i)
				_levelSavesFoldout.Add(false);

			_levelSavesFoldout[i] = EditorGUILayout.Foldout(_levelSavesFoldout[i], "Level " + (i + 1).ToString("D"), true);

			if(_levelSavesFoldout[i])
			{
				EditorGUI.indentLevel += 1;

				// Draw properties
				EditorGUILayout.PropertyField(playerPositionProp);
				EditorGUILayout.PropertyField(gameStateReferencesProp.FindPropertyRelative("Array.size"), new GUIContent("Numbers Of References"));
				EditorGUI.indentLevel += 1;

				for(int j = 0; j < gameStateReferencesProp.arraySize; j++)
				{
					EditorGUILayout.PropertyField(gameStateReferencesProp.GetArrayElementAtIndex(j));
				}

				EditorGUI.indentLevel -= 1;
				EditorGUI.indentLevel -= 1;
			}
		}
	}
}
#endif
