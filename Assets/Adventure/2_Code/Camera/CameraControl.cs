using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraControl : MonoBehaviour
{
	public static Camera active
	{
		get;
		protected set;
	}

	[SerializeField] protected float _maxTurnAngle = 0f;
	[SerializeField] protected float _turnDamping = 1f;

	protected Camera _camera;
	protected Transform _target;
	protected Quaternion _initialRotation;

	private Vector3 _lookPos;
	private Quaternion _newRotation;

	// Initialization
	void Start()
	{
		_camera = GetComponent<Camera>();
		_camera.enabled = false;

		//_initialRotation = transform.rotation;
		_initialRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

		if(CameraControl.active == null)
			CameraControl.active = Camera.main;
	}

	void Update()
	{
		if((_maxTurnAngle > 0f) && (_target != null))
		{
			_lookPos = _target.position - transform.position;

			_newRotation = Quaternion.LookRotation(_lookPos);
			_newRotation = Quaternion.Euler(0f, _newRotation.eulerAngles.y, 0f);

			float angle = Mathf.Abs(Quaternion.Angle(_newRotation, _initialRotation));

			//if(angle <= (_maxTurnAngle / 2f))
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, _newRotation, Time.deltaTime * _turnDamping);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			CameraControl.active.enabled = false;
			_camera.enabled = true;
			_target = other.transform;
			CameraControl.active = _camera;
		}
	}
}
