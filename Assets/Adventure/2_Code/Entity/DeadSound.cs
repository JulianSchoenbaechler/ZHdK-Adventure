using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Audio;

namespace Adventure.Entity
{
	[RequireComponent(typeof(AudioSource))]
	public class DeadSound : MonoBehaviour
	{
		[SerializeField] protected AudioClip[] clips;

		protected virtual void Start()
		{
			GetComponent<AudioSource>().PlayRandomizedShot(0f, clips);
		}
	}
}
