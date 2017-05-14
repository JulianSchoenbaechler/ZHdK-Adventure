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
		[SerializeField] protected UISpray _shotNoise;

		protected Animator _animator;
		protected SnapPanel _snapPanel;

		private float _timer = 0f;
		private Quaternion _rotateToTarget;
		private RaycastHit _shotHit;


		void Start()
		{
			_animator = GetComponent<Animator>();
			_snapPanel = GetComponentInChildren<SnapPanel>();
			_snapPanel.Invoke("StartSnapping", _pauseInterval + 2f);
		}
		

		void Update()
		{
			//Debug.DrawRay(_bulletSpawnPosition.position, _target.position - _bulletSpawnPosition.position + Vector3.up * _yOffset, Color.red);

			if(!_active)
				return;

			if(Vector3.Distance(transform.position, _target.position) > _maxDistance)
				return;

			// Rotate to target
			_rotateToTarget = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, _rotateToTarget, Time.deltaTime * _turnSpeed);

			// Idle, Load, Shoot
			if(_animator.GetBool("Searching"))
			{
				// Searching...


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
			}
		}

		public void OnReloadGun()
		{
			//GetComponentInChildren<SnapPanel>().StartSnapping();
			print("reload");
		}

		public void OnShootGun()
		{
			_visualShotAnimator.SetTrigger("VisualShot");
			_shotNoise.Play();

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
					print("Dead");
				}
				else
				{
					print("Missed");
				}
			}

			_snapPanel.Invoke("StartSnapping", _pauseInterval + 1.3f);
		}


		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}
	}
}
