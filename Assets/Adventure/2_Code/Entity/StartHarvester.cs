using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Audio;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Camera))]
	public class StartHarvester : MonoBehaviour
	{
		[SerializeField] protected GameObject _harvester;
		[SerializeField] protected AudioClip _indianaSheep;

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
				GameObject.FindWithTag("AmbientSound").GetComponent<AudioSource>().clip = _indianaSheep;
				GameObject.FindWithTag("AmbientSound").GetComponent<AudioSource>().Play();
			}
			/*else if(!_camera.enabled && _harvester.activeInHierarchy)
			{
				_harvester.SetActive(false);
			}*/
		}
	}
}
