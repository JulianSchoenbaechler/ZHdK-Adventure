using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.SnapPanel;
using Adventure.CameraHandling;
using Adventure.UI;

namespace Adventure.Character
{
	[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(Animator))]
	public class FarmerShootController : MonoBehaviour
	{
		[SerializeField] protected bool _active = true;
		[SerializeField] protected Transform _target;
		[SerializeField] protected float _maxDistance = 10f;
		[SerializeField] protected float _yOffset = 0f;
		[SerializeField] protected float _pauseInterval = 5f;
		[SerializeField] protected float _searchTime = 1f;
		[SerializeField] protected float _loadTime = 1f;
		[SerializeField] protected float _turnSpeed = 1f;
		[SerializeField] protected Transform _bulletSpawnPosition;
		[SerializeField] protected Animator _visualShotAnimator;
		[SerializeField] protected Animator _visualHitAnimator;
		[SerializeField] protected UISpray _loadNoise;
		[SerializeField] protected UISpray _shotNoise;
		[SerializeField] protected AudioClip _searchingAudio;
		[SerializeField] protected AudioClip _loadingAudio;
		[SerializeField] protected AudioClip _shotAudio;

		protected Animator _animator;
		protected SnapPanel _snapPanel;

		private float _timer = 0f;
		private Quaternion _rotateToTarget;
		private Vector3 _tempRotationMask;
		private RaycastHit _shotHit;
		private bool _audioPlayed = false;
		private WorldspaceImage _curseImage;


		void Start()
		{
			_animator = GetComponent<Animator>();
			_snapPanel = GetComponentInChildren<SnapPanel>();
			_curseImage = GetComponent<WorldspaceImage>();

			if(_active)
				_snapPanel.Invoke("StartSnapping", _pauseInterval + 2f);
		}
		

		void Update()
		{
			// Debug.DrawRay(_bulletSpawnPosition.position, _target.position - _bulletSpawnPosition.position + Vector3.up * _yOffset, Color.red);

			if(!_active &&  !_animator.GetBool("Searching"))
				return;

			if(Vector3.Distance(transform.position, _target.position) > _maxDistance)
				return;

			// Rotate to target
			_rotateToTarget = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
			_tempRotationMask = _rotateToTarget.eulerAngles;
			_tempRotationMask.x = 0f;
			_tempRotationMask.z = 0f;
			_rotateToTarget = Quaternion.Euler(_tempRotationMask);
			transform.rotation = Quaternion.Lerp(transform.rotation, _rotateToTarget, Time.deltaTime * _turnSpeed);

			// Idle, Load, Shoot
			if(_animator.GetBool("Searching"))
			{
				// Searching...
				if(!_audioPlayed)
				{
					GetComponent<AudioSource>().PlayOneShot(_searchingAudio);
					_audioPlayed = true;
				}

				if(_animator.GetBool("Set"))
				{
					// Ready to shoot...

					if(_timer >= _loadTime)
					{
						// Shoot!
						_animator.SetTrigger("Shoot");
						_animator.SetBool("Searching", false);
						_animator.SetBool("Set", false);
						_timer = 0f;
					}
					else
					{
						_timer += Time.deltaTime;
					}
				}
				else
				{
					// Ready to load...

					if(_timer >= _searchTime)
					{
						// Load!
						_animator.SetBool("Set", true);
						_timer = 0f;
					}
					else
					{
						_timer += Time.deltaTime;
					}
				}
			}
			else
			{
				// Idle...

				if(_timer >= _pauseInterval)
				{
					// Start searching!
					_animator.SetBool("Searching", true);
					_timer = 0f;
				}
				else
				{
					_timer += Time.deltaTime;
				}

				if((_timer >= 3f) && _curseImage.enabled)
				{
					_curseImage.enabled = false;
				}
			}
		}

		public void OnReloadGun()
		{
			GetComponent<AudioSource>().PlayOneShot(_loadingAudio);
			_loadNoise.Play();
			//print("reload");
		}

		public void OnShootGun()
		{
			_visualShotAnimator.SetTrigger("VisualShot");
			_shotNoise.Play();
			GetComponent<AudioSource>().PlayOneShot(_shotAudio);

			if(Physics.Raycast(
				_bulletSpawnPosition.position,
				_target.position - _bulletSpawnPosition.position + Vector3.up * _yOffset,
				out _shotHit,
				_maxDistance
			))
			{
				_visualHitAnimator.transform.position = _shotHit.point;
				_visualHitAnimator.transform.rotation = CameraControl.active.transform.rotation;
				_visualHitAnimator.SetTrigger("VisualHit");

				if(_shotHit.transform.CompareTag("Player"))
				{
					//print("Dead");
					//GameObject.Find("GameOver").GetComponent<UnityEngine.UI.Text>().enabled = true;	// Debug... Need fix
					_active = false;
					_target.GetComponent<SheepController>().Dead = true;
				}
				else
				{
					//print("Missed");
					_curseImage.enabled = true;
				}
			}

			if(_active == true)
				_snapPanel.Invoke("StartSnapping", _pauseInterval + 1.3f);

			_audioPlayed = false;
		}


		public bool Active
		{
			get { return _active; }
			set
			{ 
				_active = value;
				_animator.SetBool("Searching", false);
				_animator.SetBool("Set", false);
				_timer = 0f;

				if(_active == true)
					_snapPanel.Invoke("StartSnapping", _pauseInterval + 1.3f);
			}
		}
	}
}
