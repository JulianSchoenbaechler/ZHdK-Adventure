using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using JulianSchoenbaechler.GameState;

namespace Adventure.Audio
{
	public class AmbientSound : MonoBehaviour
	{
		[SerializeField] protected AudioClip _level12Ambient;
		[SerializeField] protected AudioClip _level3Ambient;
		[SerializeField] protected AudioClip _crows;

		public void ChangeAmbient(uint level)
		{
			switch(level)
			{
				case 0:
				case 1:
					GetComponent<AudioSource>().clip = _level12Ambient;
					break;

				default:
					GetComponent<AudioSource>().clip = _level3Ambient;
					GetComponent<AudioSource>().PlayOneShot(_crows);
					break;
			}
			GetComponent<AudioSource>().Play();
		}
	}
}
