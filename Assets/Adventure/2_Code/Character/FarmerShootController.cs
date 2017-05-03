﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.SnapPanel;
using Adventure.CameraHandling;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(Animator))]
public class FarmerShootController : MonoBehaviour
{
	[SerializeField] protected bool _active = true;
	[SerializeField] protected Transform _target;
	[SerializeField] protected float _maxDistance = 10f;
	[SerializeField] protected float _yOffset = 0f;
	[SerializeField] protected float _pauseInterval = 5f;
	[SerializeField] protected float _reloadInterval = 1f;
	[SerializeField] protected float _turnSpeed = 1f;
	[SerializeField] protected Transform _bulletSpawnPosition;
	[SerializeField] protected Animator _visualShotAnimator;
	[SerializeField] protected Animator _visualHitAnimator;

	protected Animator _animator;
	protected SnapPanel _snapPanel;

	private float _timer = 0f;
	private Quaternion _rotateToTarget;
	private RaycastHit _shotHit;

	// Use this for initialization
	void Start()
	{
		_animator = GetComponent<Animator>();
		_snapPanel = GetComponentInChildren<SnapPanel>();
		_snapPanel.Invoke("StartSnapping", _pauseInterval - 1f);
	}
	
	// Update is called once per frame
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
		if(_animator.GetBool("Set"))
		{
			if(_timer >= _reloadInterval)
			{
				_animator.SetTrigger("Shoot");
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
			if(_timer >= _pauseInterval)
			{
				_animator.SetBool("Set", true);
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
	}

	public void OnShootGun()
	{
		_visualShotAnimator.SetTrigger("VisualShot");

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

		_snapPanel.Invoke("StartSnapping", _pauseInterval - 1f);
	}


	public bool Active
	{
		get { return _active; }
		set { _active = value; }
	}
}
