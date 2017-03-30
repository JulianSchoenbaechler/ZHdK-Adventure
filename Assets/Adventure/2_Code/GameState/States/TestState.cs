using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.GameState;

[CustomGameState]
public class TestState : IGameState
{
	public string name { get; set; }

	public void Start()
	{
		Debug.Log("Start");
		GameState.active = "MyState";
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
}
