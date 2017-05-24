using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class Playmode
{
	public enum PlaymodeState
	{
		Stopped,
		Playing,
		Paused,
		AboutToPlay
	}

	/// <summary>
	/// The current editor playmode state.
	/// </summary>
	public static PlaymodeState currentPlaymode;

	/// <summary>
	/// Occurs when playmode state changed.
	/// </summary>
	public static event Action<PlaymodeState> playmodeStateChanged;

	/// <summary>
	/// Initializes the static <see cref="Playmode"/> class.
	/// </summary>
	static Playmode()
	{
		currentPlaymode = PlaymodeState.Stopped;
		EditorApplication.playmodeStateChanged += OnPlaymodeStateChange;
	}

	/// <summary>
	/// Raises the playmode state change event.
	/// </summary>
	private static void OnPlaymodeStateChange()
	{
		// Change playmode enum
		currentPlaymode = PlaymodeState.Stopped;

		if(EditorApplication.isPaused)
			currentPlaymode = PlaymodeState.Paused;

		if(EditorApplication.isPlayingOrWillChangePlaymode)
			currentPlaymode = PlaymodeState.AboutToPlay;

		if(EditorApplication.isPlaying)
			currentPlaymode = PlaymodeState.Playing;

		// Fire on playmode state change event
		if(playmodeStateChanged != null)
			playmodeStateChanged(currentPlaymode);
	}
}
