using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using JulianSchoenbaechler.GameState;

[CustomGameState]
public class Level2State : IGameState
{
	public string name { get; set; }

	public void Start()
	{
		Debug.Log("Start");
		SceneManager.sceneLoaded -= SceneLoadedHandler;
		SceneManager.sceneLoaded += SceneLoadedHandler;
	}

	public void Update()
	{
	}

	public void LateUpdate()
	{
	}

	public void FixedUpdate()
	{
	}

	public void ResetState()
	{
		Debug.Log("Reset");
	}



	private void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("After scene load...");
	}
}
