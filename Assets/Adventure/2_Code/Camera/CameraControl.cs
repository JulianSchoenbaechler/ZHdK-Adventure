using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.Core;

[RequireComponent(typeof(Rigidbody))]
public class CameraControl : MonoBehaviour
{
	public static Camera active									// Active camera
	{
		get;
		protected set;
	}

	[SerializeField] protected float _maxTurnAngle = 0f;		// Maximal turn angle in degree
	[SerializeField] protected float _turnSpeed = 1f;			// Turn speed

	protected Camera _camera;									// Camera component
	protected Transform _target;								// Target transform
	protected Quaternion _initialRotation;						// Initial rotation

	private Vector3 _lookPos;
	private Quaternion _newRotation;
	private TransformClone[] _transformClones;


	/// <summary>
	/// On awake.
	/// </summary>
	private void Awake()
	{
		if(CameraControl.active == null)
			CameraControl.active = Camera.main;
	}

	/// <summary>
	/// Initialization.
	/// </summary>
	private void Start()
	{
		_camera = GetComponent<Camera>();
		_camera.enabled = false;

		_initialRotation = transform.rotation;

		KeepChildTransform(true);
	}


	/// <summary>
	/// Every frame.
	/// </summary>
	private void Update()
	{
		// Has target and a turn angle?
		if((_maxTurnAngle > 0f) && (_target != null))
		{
			// Targetting
			_lookPos = _target.position - transform.position;

			// Rotate to look at target
			_newRotation = Quaternion.LookRotation(_lookPos);

			// Modify rotation -> only rotate around y-axis (euler)
			_newRotation = Quaternion.Euler(
				_initialRotation.eulerAngles.x,
				Mathf.Clamp(
					_newRotation.eulerAngles.y,
					_initialRotation.eulerAngles.y - (_maxTurnAngle / 2f),
					_initialRotation.eulerAngles.y + (_maxTurnAngle / 2f)),
				_initialRotation.eulerAngles.z);

			// Apply transform
			transform.rotation = Quaternion.Slerp(transform.rotation, _newRotation, Time.deltaTime * _turnSpeed);
			KeepChildTransform(false);
		}
	}

	/// <summary>
	/// When getting triggered.
	/// </summary>
	protected void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			CameraControl.active.enabled = false;		// Deactivate active camera
			_camera.enabled = true;						// Activate this camera
			_target = other.transform;					// Target the player
			CameraControl.active = _camera;				// Set this camera as new globally active camera
		}
	}

	/// <summary>
	/// Keeps the child transform.
	/// </summary>
	/// <param name="overrideTransform">If set to <c>true</c> override transform.</param>
	private void KeepChildTransform(bool overrideTransform)
	{
		// Set transform?
		if(overrideTransform)
		{
			_transformClones = GetComponentsInChildren<Transform>().CloneTransform();
		}
		else
		{
			Transform[] children = GetComponentsInChildren<Transform>();

			for(int i = 0; i < _transformClones.Length; i++)
			{
				if(!children[i].CompareTag("Camera"))
				{
					_transformClones[i].ApplyTransform(children[i]);
				}
			}
		}
	}
}
