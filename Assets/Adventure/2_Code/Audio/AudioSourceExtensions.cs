using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Audio
{
	public static class AudioSourceExtensions
	{
		/// <summary>
		/// Plays a random AudioClip with slightly randomized pitch value.
		/// </summary>
		/// <param name="clips">An array of clips that could be played.</param>
		public static void PlayRandomizedShot(this AudioSource source, AudioClip[] clips)
		{
			//ShuffleArray<AudioClip>(out clips);

			source.pitch = Random.Range(0.9f, 1.1f);
			source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
		}

		/// <summary>
		/// Plays a random AudioClip with slightly randomized pitch value.
		/// </summary>
		/// <param name="clips">An array of clips that could be played.</param>
		/// <param name="volumeScale">The scale of the volume (0-1).</param>
		public static void PlayRandomizedShot(this AudioSource source, AudioClip[] clips, float volumeScale)
		{
			//ShuffleArray<AudioClip>(out clips);

			source.pitch = Random.Range(0.9f, 1.1f);
			source.PlayOneShot(clips[Random.Range(0, clips.Length)], volumeScale);
		}

		/// <summary>
		/// Plays a random AudioClip with randomized pitch value.
		/// </summary>
		/// <param name="randomStrength">The strength of how extreme the pitch should be randomized.</param>
		/// <param name="clips">An array of clips that could be played.</param>
		public static void PlayRandomizedShot(this AudioSource source, float randomStrength, AudioClip[] clips)
		{
			//ShuffleArray<AudioClip>(out clips);
			randomStrength = Mathf.Clamp01(randomStrength);

			source.pitch = 1f + Random.Range(-3f, 3f) * randomStrength;
			source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
		}

		/// <summary>
		/// Plays a random AudioClip with randomized pitch value.
		/// </summary>
		/// <param name="randomStrength">The strength of how extreme the pitch should be randomized.</param>
		/// <param name="clips">An array of clips that could be played.</param>
		/// <param name="volumeScale">The scale of the volume (0-1).</param>
		public static void PlayRandomizedShot(this AudioSource source, float randomStrength, AudioClip[] clips, float volumeScale)
		{
			//ShuffleArray<AudioClip>(out clips);
			randomStrength = Mathf.Clamp01(randomStrength);

			source.pitch = 1f + Random.Range(-3f, 3f) * randomStrength;
			source.PlayOneShot(clips[Random.Range(0, clips.Length)], volumeScale);
		}


		/// <summary>
		/// Shuffles an array of any type (Knuth algorithm).
		/// </summary>
		/// <param name="array">Existing array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		/*
		private static void ShuffleArray<T>(out T[] array)
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
		*/
	}
}
