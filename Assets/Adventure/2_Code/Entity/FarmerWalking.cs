using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Entity
{
	public class FarmerWalking : MonoBehaviour
	{
		[SerializeField] protected float _animationSpeed = 2f;
		[SerializeField] protected float _speed = 1f;
		[SerializeField] protected float _destroyTime = 5f;

		protected Vector3 _initialPosition;

		protected virtual void Start()
		{
			_initialPosition = transform.position;
			GetComponent<Animator>().speed = _animationSpeed;
			Invoke("Reset", _destroyTime);
		}


		protected virtual void Update()
		{
			transform.position += transform.forward * Time.deltaTime * _speed;
		}


		protected virtual void Reset()
		{
			transform.position = _initialPosition;
			gameObject.SetActive(false);
		}
	}
}
