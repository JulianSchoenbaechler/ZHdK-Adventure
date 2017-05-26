using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Adventure.LevelManager
{
	public class LevelManager : MonoBehaviour
	{
		[Serializable]
		private sealed class LevelSave
		{
			[SerializeField, HideInInspector] private Transform _playerPosition;
			[SerializeField, HideInInspector] private GameObject[] _gameStateReferences;

			public Vector3 PlayerPosition
			{
				get { return _playerPosition.position; }
			}

			public GameObject GetReference(uint index)
			{
				return _gameStateReferences.Length > index ? _gameStateReferences[index] : null;
			}
		}


		// This instance
		private static LevelManager _instance;

		[SerializeField]  private GameObject _playerObject;
		[SerializeField]  private LevelSave[] _levelSaves;

		public static GameObject Player
		{
			get { return LevelManager._instance._playerObject; }
		}


		private void Awake()
		{
			LevelManager._instance = this;
		}

		/// <summary>
		/// Gets the player position for a specific level.
		/// </summary>
		/// <returns>The player position.</returns>
		/// <param name="levelIndex">Level index (starting from 0).</param>
		public static Vector3 GetPlayerPosition(uint levelIndex)
		{
			if(levelIndex < LevelManager._instance._levelSaves.Length)
				return LevelManager._instance._levelSaves[levelIndex].PlayerPosition;

			return default(Vector3);
		}

		/// <summary>
		/// Gets a specific reference from level.
		/// </summary>
		/// <returns>The requested reference.</returns>
		/// <param name="levelIndex">Level index (starting from 0).</param>
		/// <param name="referenceIndex">Reference index (starting from 0).</param>
		public static GameObject GetReference(uint levelIndex, uint referenceIndex)
		{
			if(levelIndex < LevelManager._instance._levelSaves.Length)
				return LevelManager._instance._levelSaves[levelIndex].GetReference(referenceIndex);

			return null;
		}
	}
}
