using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Audio;

namespace Adventure.Entity
{
	public class FarmerWalking : MonoBehaviour
	{
		[SerializeField] protected float _animationSpeed = 2f;
		[SerializeField] protected float _speed = 1f;
		[SerializeField] protected float _destroyTime = 5f;
		[SerializeField] protected AudioClip[] _footsteps;
		[SerializeField] protected float _footstepsDelay = 0.3f;

		protected Vector3 _initialPosition;
		private float _time = 0f;

		protected virtual void Start()
		{
			_initialPosition = transform.position;
			GetComponent<Animator>().speed = _animationSpeed;
			Invoke("Reset", _destroyTime);
		}


		protected virtual void Update()
		{
			transform.position += transform.forward * Time.deltaTime * _speed;

			if(_time >= _footstepsDelay)
			{
				GetComponent<AudioSource>().PlayRandomizedShot(0.1f, _footsteps);
				_time = 0f;
			}
			else
			{
				_time += Time.deltaTime;
			}
		}


		protected virtual void Reset()
		{
			transform.position = _initialPosition;
			gameObject.SetActive(false);
		}
	}
}
