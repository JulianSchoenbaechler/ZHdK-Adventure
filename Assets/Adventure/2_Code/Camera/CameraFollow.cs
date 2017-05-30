using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.Core;

namespace Adventure.CameraHandling
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField] protected bool _active = false;
		[SerializeField] protected Transform _target;

		private Vector3 _initialPosition;
		private Vector3 _differenceVector;
		private Vector3 _newPosition;


		protected void Start()
		{
			_initialPosition = transform.position;
			_differenceVector = _target.position - transform.position;
		}


		protected void Update()
		{
			if(_active)
			{
				_newPosition.Set(
					_target.position.x + Mathf.Abs(_differenceVector.x),
					_initialPosition.y,
					_target.position.z
				);

				transform.position = _newPosition;
			}

			if(GetComponent<Camera>().enabled && !_active)
			{
				_initialPosition = transform.position;
				_differenceVector = _target.position - transform.position;
				_active = true;
			}
			else if(!GetComponent<Camera>().enabled && _active)
			{
				_active = false;
			}
		}
	}
}
