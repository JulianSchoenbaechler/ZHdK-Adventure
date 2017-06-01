using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using JulianSchoenbaechler.GameState;

namespace Adventure.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class AmbientSound : MonoBehaviour
	{
		[SerializeField] protected AudioMixerSnapshot _insideMix;
		[SerializeField] protected AudioMixerSnapshot _outsideMix;

		protected virtual void Start()
		{
			if(GameState.active == "Level1State")
			{
				_insideMix.TransitionTo(0.1f);
			}
			else
			{
				_outsideMix.TransitionTo(0.1f);
			}
		}

		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.CompareTag("Player"))
			{
				
			}
		}
	}
}
