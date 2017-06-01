using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Adventure.LevelManager;
using Adventure.Character;
using Adventure.UI;
using JulianSchoenbaechler.GameState;

[CustomGameState]
public class Level3State : IGameState
{
	protected uint _levelIndex = 2;

	public string name { get; set; }

	public void Start()
	{
		SceneManager.sceneLoaded -= SceneLoadedHandler;
		SceneManager.sceneLoaded += SceneLoadedHandler;

		LevelManager.GetReference(_levelIndex, 1).SetActive(true);
	}

	public void Update()
	{
//#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			GameState.active = "Level1State";
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			GameState.active = "Level2State";
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
//#endif
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
		LevelManager.GetReference(_levelIndex, 1).SetActive(false);
	}



	private void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
	{
		GameObject player = GameObject.FindWithTag("Player");

		// Level 2 references
		LevelManager.GetReference(_levelIndex - 1, 1).GetComponent<Animator>().SetTrigger("Open");		// Open barn door 1
		LevelManager.GetReference(_levelIndex - 1, 2).GetComponent<Animator>().SetTrigger("Open");		// Open barn door 2
		LevelManager.GetReference(_levelIndex - 1, 3).GetComponent<Animator>().enabled = true;			// Close fence
		player.transform.position = LevelManager.GetPlayerPosition(_levelIndex);						// Move player
		player.GetComponent<SheepController>().InBarnFootsteps = false;

		LevelManager.GetReference(_levelIndex, 0).SetActive(false);										// Close fence
		LevelManager.GetReference(_levelIndex, 1).SetActive(true);
	}
}
