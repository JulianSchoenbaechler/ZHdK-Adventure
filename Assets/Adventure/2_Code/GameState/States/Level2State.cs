using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Adventure.LevelManager;
using Adventure.Character;
using Adventure.UI;
using JulianSchoenbaechler.GameState;

[CustomGameState]
public class Level2State : IGameState
{
	protected uint _levelIndex = 1;

	public string name { get; set; }

	public void Start()
	{
		SceneManager.sceneLoaded -= SceneLoadedHandler;
		SceneManager.sceneLoaded += SceneLoadedHandler;
	}

	public void Update()
	{
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			GameState.active = "Level1State";
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			GameState.active = "Level3State";
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
#endif
	}

	public void LateUpdate()
	{
	}

	public void FixedUpdate()
	{
	}

	public void ResetState()
	{
		SceneManager.sceneLoaded -= SceneLoadedHandler;
	}



	private void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
	{
		GameObject player = GameObject.FindWithTag("Player");

		LevelManager.GetReference(_levelIndex, 0).SetActive(true);									// Activate farmer
		LevelManager.GetReference(_levelIndex, 1).GetComponent<Animator>().SetTrigger("Open");		// Open barn door 1
		LevelManager.GetReference(_levelIndex, 2).GetComponent<Animator>().SetTrigger("Open");		// Open barn door 2
		LevelManager.GetReference(_levelIndex, 3).GetComponent<Animator>().enabled = true;			// Close fence
		player.transform.position = LevelManager.GetPlayerPosition(1);								// Move player
		player.GetComponent<FarmerNavigator>().enabled = true;
	}
}
