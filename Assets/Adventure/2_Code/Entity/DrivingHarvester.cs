using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.SnapPanel;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Animator))]
	public class DrivingHarvester : MonoBehaviour
	{
		[SerializeField] protected Transform _target;
		[SerializeField] protected float _turnSpeed = 1f;

		protected Animator _animator;
		private Quaternion _rotateToTarget;
		private Vector3 _tempRotationMask;


		protected virtual void Start()
		{
			_animator = GetComponent<Animator>();
		}

		protected virtual void Update()
		{
			// Rotate to target
			_rotateToTarget = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
			_tempRotationMask = _rotateToTarget.eulerAngles;
			_tempRotationMask.x = 0f;
			_tempRotationMask.z = 0f;
			_rotateToTarget = Quaternion.Euler(_tempRotationMask);
			transform.rotation = Quaternion.Lerp(transform.rotation, _rotateToTarget, Time.deltaTime * _turnSpeed);
		}
	}
}
