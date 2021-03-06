﻿namespace JulianSchoenbaechler.GameState
{
	public interface IGameState
	{
		string name { get; set; }

		void ResetState();
		void Start();
		void Update();
		void LateUpdate();
		void FixedUpdate();
	}
}
