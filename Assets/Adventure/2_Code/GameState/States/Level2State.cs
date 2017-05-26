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
		SceneManager.sceneLoaded -= SceneLoadedHandler;
	}



	private void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("After scene load...");
	}
}
