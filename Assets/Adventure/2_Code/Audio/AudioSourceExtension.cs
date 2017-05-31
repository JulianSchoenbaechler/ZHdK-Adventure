using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Audio
{
	public static class AudioSourceExtension
	{
		AudioSource test;

		// Use this for initialization
		void Start () {
			AudioClip x;
			test.PlayOneShot(x);
		}
		
		public static void PlayRandomizedShot(this AudioSource source, AudioClip[] clips)
		{
			clips = ShuffleArray<AudioClip>(clips);
		}


		/// <summary>
		/// Shuffles an array of any type.
		/// </summary>
		/// <returns>The arraywith type T.</returns>
		/// <param name="array">Existing array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		private static T[] ShuffleArray<T>(T[] array)
		{
			// Knuth shuffle algorithm
			for(int t = 0; t < array.Length; t++)
			{
				T tmp = array[t];
				int r = Random.Range(t, array.Length);
				array[t] = array[r];
				array[r] = tmp;
			}

			return array;
		}
	}
}
