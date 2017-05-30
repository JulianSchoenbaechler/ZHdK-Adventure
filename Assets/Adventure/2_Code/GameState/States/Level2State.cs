using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	protected GameObject _player;
	protected readonly string[] _uiCount = {
		"0 <i>I</i><size=\"40\"> </size> 3",
		"1 <i>I</i><size=\"40\"> </size> 3",
		"2 <i>I</i><size=\"40\"> </size> 3",
		"3 <i>I</i><size=\"40\"> </size> 3"
	};
	protected int _uiCountIndex = 0;

	public void Start()
	{
		SceneManager.sceneLoaded -= SceneLoadedHandler;
		SceneManager.sceneLoaded += SceneLoadedHandler;

		_player = GameObject.FindWithTag("Player");
		_player.GetComponent<FarmerNavigator>().enabled = true;

		LevelManager.GetReference(_levelIndex, 4).SetActive(true);
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
		LevelManager.GetReference(_levelIndex, 4).SetActive(false);
	}

	public void SheepIncrement()
	{
		if(_uiCountIndex < _uiCount.Length)
			_uiCountIndex++;

		LevelManager.GetReference(_levelIndex, 4).GetComponent<Text>().text = _uiCount[_uiCountIndex];

		// Destroy farmer when last sheep returned
		if(_uiCountIndex >= 2)
		{
			GameObject.FindWithTag("Player").GetComponent<FarmerNavigator>().enabled = false;
			GameObject.Destroy(LevelManager.GetReference(_levelIndex, 0));							// Destroy farmer
		}
	}

	private void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
	{
		_player = GameObject.FindWithTag("Player");

		LevelManager.GetReference(_levelIndex, 0).SetActive(true);									// Activate farmer
		LevelManager.GetReference(_levelIndex, 1).GetComponent<Animator>().SetTrigger("Open");		// Open barn door 1
		LevelManager.GetReference(_levelIndex, 2).GetComponent<Animator>().SetTrigger("Open");		// Open barn door 2
		LevelManager.GetReference(_levelIndex, 3).GetComponent<Animator>().enabled = true;			// Close fence
		_player.transform.position = LevelManager.GetPlayerPosition(_levelIndex);					// Move player
		_player.GetComponent<FarmerNavigator>().enabled = true;
	}
}
