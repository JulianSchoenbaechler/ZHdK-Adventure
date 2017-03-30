using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class SheepController : MonoBehaviour
{
	[SerializeField] protected float _walkSpeed = 1f;
	[SerializeField] protected float _runSpeed = 1f;
	[SerializeField] protected float _turnSpeed = 1f;
	[SerializeField] protected float _jumpForce = 1f;
	[SerializeField] protected float _gravity = 1f;

	private float _jumpCooldown = 0.1f;
	private float _time = 0f;

	public bool IsGrounded { get; set; }

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

		transform.Rotate(Vector3.forward * horizontalAxis * _turnSpeed * Time.deltaTime * 100f);

		/*
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x,
				-verticalAxis * _runSpeed * Time.deltaTime * 100f,
				_rigidbody.velocity.z);
		else
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x,
				-verticalAxis * _walkSpeed * Time.deltaTime * 100f,
				_rigidbody.velocity.z);
		*/
		
		if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
		{
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
			_time = Time.time + _jumpCooldown;
			IsGrounded = false;
			//IsGrounded = false;
			//_ignoreGrounded = 5;
		}

		Vector3 currentVelocity = _rigidbody.velocity;
		currentVelocity.x = verticalAxis * _walkSpeed;
		/*
		// Collision fix: isGrounded detection
		if(_ignoreGrounded > 0)
			_ignoreGrounded--;

		if(!IsGrounded)
			_rigidbody.velocity -= Vector3.up * _gravity * Time.deltaTime * 100f;
		*/
	}


	// Collision handling
	void OnCollisionStay(Collision collisionInfo)
	{
		bool collisionFeet = false;

		foreach(ContactPoint contact in collisionInfo.contacts)
		{
			if(contact.normal.y > 0f)
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
	/*
	void OnCollisionExit(Collision collisionInfo)
	{
		IsGrounded = false;
	}
	*/
}
