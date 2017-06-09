using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Adventure.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class RandomBark : MonoBehaviour
	{
		[SerializeField] protected AudioClip[] _clips;
		[SerializeField] protected float _minPause = 2f;
		[SerializeField] protected float _maxPause = 10f;

		protected AudioSource _audioSource;
		private float _time = 0f;
		private float _pause = 0f;


		private void Start()
		{
			_audioSource = GetComponent<AudioSource>();

			_pause = Random.Range(_minPause, _maxPause);
		}


		protected virtual void Update()
		{
			if(_time >= _pause)
			{
				_audioSource.PlayRandomizedShot(_clips);
				_pause = Random.Range(_minPause, _maxPause);
				_time = 0f;
			}
			else
			{
				_time += Time.deltaTime;
			}
		}
	}
}
