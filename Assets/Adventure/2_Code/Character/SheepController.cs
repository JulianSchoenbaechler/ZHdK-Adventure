using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Adventure.UI;
using JulianSchoenbaechler.GameState;

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
		[SerializeField] protected GameObject _deadPanel;

		private float _jumpCooldown = 0.1f;
		private float _time = 0f;
		private Vector3 _currentVelocity;
		private Vector3 _velocityDir;

		private bool _dead = false;
		private bool _fadeout = false;
		private Image _deadPanelImage;
		private Color _deadPanelColor;
		private float _deadColorLerp = 0f;

		public bool IsGrounded { get; private set; }
		public bool Dead
		{
			get { return _dead; }
			set
			{
				_dead = value;
				StartCoroutine(Die());
			}
		}

		protected Rigidbody _rigidbody;
		protected Camera _deathCam;



		void Start()
		{
			IsGrounded = false;
			_rigidbody = GetComponent<Rigidbody>();
			_deathCam = GetComponentInChildren<Camera>();
			_deathCam.enabled = false;

			_deadPanelColor = new Color(0f, 0f, 0f, 0f);
			_deadPanelImage = _deadPanel.GetComponent<Image>();
			_deadPanelImage.color = _deadPanelColor;
			_deadPanel.SetActive(false);
		}

		void Update()
		{
			// Fade-out when dead
			if(_dead)
			{
				if(_fadeout)
				{
					if(!_deadPanel.activeInHierarchy)
						_deadPanel.SetActive(true);
					
					_deadPanelImage.color = Color.Lerp(_deadPanelColor, Color.black, _deadColorLerp);
					_deadColorLerp = _deadColorLerp + (Time.unscaledDeltaTime * 0.3f);
				}
				return;
			}

			// Character-controller
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

		// Death
		IEnumerator Die()
		{
			GetComponent<FarmerNavigator>().enabled = false;
			Time.timeScale = 0.2f;
			_deathCam.enabled = true;

			yield return new WaitForSecondsRealtime(3f);
			_fadeout = true;
			yield return new WaitForSecondsRealtime(10f);

			// Restart level
			Time.timeScale = 1f;
			SceneManager.LoadScene(0);
		}
	}
}
