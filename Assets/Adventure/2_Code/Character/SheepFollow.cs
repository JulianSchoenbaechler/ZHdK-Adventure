using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Interaction;
using Adventure.UI;

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
		[SerializeField] protected Transform _sheepStairEntry;
		[SerializeField] protected Transform _sheepStairEndpoint;
		[SerializeField] protected float _hintDistance = 6f;

		private bool _buildStair = false;
		private bool _finishStair = false;
		private bool _jump = false;
		private float _jumpCooldown = 0.5f;
		private float _time = 0f;
		private float _speed = 0f;
		private Vector3 _currentVelocity;
		private Vector3 _velocityDir;
		protected Rigidbody _rigidbody;
		private Quaternion _rotateToTarget;
		private Vector3 _tempRotationMask;
		protected WorldspaceImage _hint;

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
			_hint = GetComponent<WorldspaceImage>();
			_hint.enabled = false;

			GetComponent<InteractionObject>().InteractionEvent += PlayerInteraction;
		}

		private void Update()
		{
			DistanceCheck();

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

		// Walk distance-check and speed calculation
		protected void DistanceCheck()
		{
			if(Vector3.Distance(transform.position, _target.position) < _hintDistance)
			{
				if(!_active)
				{
					if(!_hint.enabled)
						_hint.enabled = true;
				}
				else
				{
					if(_hint.enabled)
						_hint.enabled = false;
				}
			}
			else
			{
				if(_hint.enabled)
					_hint.enabled = false;

			}

			if(Vector3.Distance(transform.position, _target.position) < _minDistance)
			{
				// Building stairs
				if(_buildStair)
				{
					_buildStair = false;
					_finishStair = true;
					_target = _sheepStairEndpoint;
					_rigidbody.useGravity = false;
					GetComponentInChildren<Collider>().enabled = false;
				}
				else if(_finishStair)
				{
					_finishStair = false;
					Destroy(gameObject);
				}

				// Slow down
				_speed = Mathf.Lerp(_speed, 0f, Time.deltaTime * 100f);
			}
			else
			{
				if(!_active)
					_speed = Mathf.Lerp(_speed, 0f, Time.deltaTime * 100f);
				else
					_speed = Mathf.Lerp(_speed, -1f, Time.deltaTime * 100f);
			}
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


		// Trigger handling
		protected virtual void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("SheepStair"))
			{
				_target = _sheepStairEntry;
				_buildStair = true;
			}
		}


		public void PlayerInteraction()
		{
			_active = true;
		}
	}
}
