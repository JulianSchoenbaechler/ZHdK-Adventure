﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Interaction;

namespace Adventure.Character
{
	[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(InteractionObject))]
	public class SheepFollow : MonoBehaviour
	{
		public enum CharacterTransform
		{
			xAxisUp,
			yAxisUp,
			zAxisUp
		}

		public CharacterTransform characterTransform;
		[SerializeField] protected bool _active = false;
		[SerializeField] protected Transform _target;
		[SerializeField] protected float _minDistance = 0.5f;
		[SerializeField] protected float _walkSpeed = 1f;
		[SerializeField] protected float _turnSpeed = 1f;
		[SerializeField] protected float _jumpForce = 1f;

		private bool _jump = false;
		private float _jumpCooldown = 0.5f;
		private float _time = 0f;
		private float _speed = 0f;
		private Vector3 _currentVelocity;
		private Vector3 _velocityDir;
		protected Rigidbody _rigidbody;
		private Quaternion _rotateToTarget;
		private Vector3 _tempRotationMask;

		public bool IsGrounded { get; private set; }
		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}



		private void Start()
		{
			IsGrounded = false;
			_rigidbody = GetComponent<Rigidbody>();
			GetComponent<InteractionObject>().InteractionEvent += PlayerInteraction;
		}

		private void Update()
		{
			if(!_active)
				_speed = Mathf.Lerp(_speed, 0f, Time.deltaTime * 100f);
			else
				_speed = Mathf.Lerp(_speed, -1f, Time.deltaTime * 100f);

			_currentVelocity = _rigidbody.velocity;

			switch(characterTransform)
			{
				case CharacterTransform.xAxisUp:
					_velocityDir = _speed * _walkSpeed * transform.forward;

					// Rotate to target
					_rotateToTarget = Quaternion.LookRotation(_target.position - transform.position);
					_tempRotationMask.Set(_rotateToTarget.eulerAngles.y, -90f, 0f);
					break;

				case CharacterTransform.yAxisUp:
					_velocityDir = _speed * _walkSpeed * transform.forward;

					// Rotate to target
					_rotateToTarget = Quaternion.LookRotation(_target.position - transform.position);
					_tempRotationMask.Set(0f, _rotateToTarget.eulerAngles.y, 0f);
					break;

				default:
					_velocityDir = _speed * _walkSpeed * transform.up;

					// Rotate to target
					_rotateToTarget = Quaternion.LookRotation(_target.position - transform.position);
					_tempRotationMask.Set(-90f, 0f, _rotateToTarget.eulerAngles.y);
					break;
					
			}

			// Apply rotation
			_rotateToTarget = Quaternion.Euler(_tempRotationMask);
			transform.rotation = Quaternion.Lerp(transform.rotation, _rotateToTarget, Time.deltaTime * _turnSpeed);

			if(IsGrounded)
			{
				if(_jump)
				{
					_currentVelocity.y = _jumpForce;
					_time = Time.time + _jumpCooldown;
					IsGrounded = false;
					_jump = false;
				}

				_currentVelocity.x = _velocityDir.x;
				_currentVelocity.z = _velocityDir.z;
			}

			_rigidbody.velocity = _currentVelocity;
		}


		// Collision handling
		protected virtual void OnCollisionStay(Collision collisionInfo)
		{
			bool collisionFeet = false;

			foreach(ContactPoint contact in collisionInfo.contacts)
			{
				float angle = Vector3.Dot(contact.normal, Vector3.up);

				if(angle > 0.8f)
				{
					collisionFeet = true;
				}
				else if((angle > -0.3f) && (angle < 0.3f))
				{
					if(IsGrounded)
						_jump = true;
				}
			}

			if(collisionFeet && !IsGrounded)
			{
				// Jump cooldown...
				if(Time.time >= (_time + _jumpCooldown))
				{
					IsGrounded = true;
				}
			}
		}


		public void PlayerInteraction()
		{
			print("gu");
			_active = true;
		}
	}
}
