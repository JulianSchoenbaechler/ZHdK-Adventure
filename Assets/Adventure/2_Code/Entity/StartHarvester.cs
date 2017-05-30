using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Camera))]
	public class StartHarvester : MonoBehaviour
	{
		[SerializeField] protected GameObject _harvester;

		protected Camera _camera;


		protected virtual void Start()
		{
			_camera = GetComponent<Camera>();
		}

		protected virtual void Update()
		{
			if(_camera.enabled && !_harvester.activeInHierarchy)
			{
				_harvester.SetActive(true);
			}
			/*else if(!_camera.enabled && _harvester.activeInHierarchy)
			{
				_harvester.SetActive(false);
			}*/
		}
	}
}
