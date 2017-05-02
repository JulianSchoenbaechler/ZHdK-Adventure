using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Character
{
	[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
	public class SheepController : MonoBehaviour
	{
		public enum CharacterTransform
		{
			xAxisUp,
			yAxisUp,
			zAxisUp
		}

		public CharacterTransform characterTransform;
		[SerializeField] protected float _walkSpeed = 1f;
		[SerializeField] protected float _turnSpeed = 1f;
		[SerializeField] protected float _jumpForce = 1f;

		private float _jumpCooldown = 0.1f;
		private float _time = 0f;
		private Vector3 _currentVelocity;
		private Vector3 _velocityDir;

		public bool IsGrounded { get; private set; }

		protected Rigidbody _rigidbody;

		void Start()
		{
			IsGrounded = false;
			_rigidbody = GetComponent<Rigidbody>();
		}

		void Update()
		{
			float horizontalAxis = Input.GetAxis("Horizontal");
			float verticalAxis = Input.GetAxis("Vertical");

			_currentVelocity = _rigidbody.velocity;

			switch(characterTransform)
			{
				case CharacterTransform.xAxisUp:
					_velocityDir = -verticalAxis * _walkSpeed * transform.forward;
					transform.Rotate(Vector3.right * horizontalAxis * _turnSpeed * Time.deltaTime * 100f);
					break;

				case CharacterTransform.yAxisUp:
					_velocityDir = -verticalAxis * _walkSpeed * transform.forward;
					transform.Rotate(Vector3.up * horizontalAxis * _turnSpeed * Time.deltaTime * 100f);
					break;

				default:
					_velocityDir = -verticalAxis * _walkSpeed * transform.up;
					transform.Rotate(Vector3.forward * horizontalAxis * _turnSpeed * Time.deltaTime * 100f);
					break;
					
			}

			if(IsGrounded)
			{
				if(Input.GetKeyDown(KeyCode.Space))
				{
					_currentVelocity.y = _jumpForce;
					_time = Time.time + _jumpCooldown;
					IsGrounded = false;
				}

				_currentVelocity.x = _velocityDir.x;
				_currentVelocity.z = _velocityDir.z;
			}

			_rigidbody.velocity = _currentVelocity;
		}


		// Collision handling
		void OnCollisionStay(Collision collisionInfo)
		{
			bool collisionFeet = false;

			foreach(ContactPoint contact in collisionInfo.contacts)
			{
				if(Vector3.Dot(contact.normal, Vector3.up) > 0.8f)
					collisionFeet = true;
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
	}
}
