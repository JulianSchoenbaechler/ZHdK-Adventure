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

	protected Camera _camera;

	// Initialization
	void Start()
	{
		_camera = GetComponent<Camera>();

		_camera.enabled = false;

		if(CameraControl.active == null)
			CameraControl.active = Camera.main;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			CameraControl.active.enabled = false;
			_camera.enabled = true;
			CameraControl.active = _camera;
		}
	}
}
