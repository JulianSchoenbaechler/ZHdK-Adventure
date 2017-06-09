using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using JulianSchoenbaechler.GameState;

namespace Adventure.Audio
{
	public class AmbientDimm : MonoBehaviour
	{
		[SerializeField] protected AudioMixerSnapshot _insideMix;
		[SerializeField] protected AudioMixerSnapshot _outsideMix;

		protected virtual void Start()
		{
			if(GameState.active == "Level1State")
				_insideMix.TransitionTo(0.01f);
			else
				_outsideMix.TransitionTo(0.01f);
		}

		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.CompareTag("Player"))
			{
				_insideMix.TransitionTo(0.5f);
			}
		}

		protected virtual void OnTriggerExit(Collider collider)
		{
			if(collider.CompareTag("Player"))
			{
				_outsideMix.TransitionTo(0.5f);
			}
		}
	}
}
