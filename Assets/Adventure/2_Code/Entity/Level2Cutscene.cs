using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Character;
using Adventure.UI;
using Adventure.CameraHandling;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider))]
	public class Level2Cutscene : MonoBehaviour
	{
		[SerializeField] protected Camera _cutsceneCamera;
		[SerializeField] protected Texture[] _worldspaceImages;
		[SerializeField] protected float _imageDisplayDelay = 3f;
		[SerializeField] protected float _imageDisplayTime = 3f;
		[SerializeField] protected GameObject _farmer;

		private WaitForSeconds _timeDelay, _timeBetween;

		protected void Start()
		{
			_cutsceneCamera.enabled = false;
			_cutsceneCamera.GetComponent<AudioListener>().enabled = false;
			_farmer.SetActive(false);
			_timeDelay = new WaitForSeconds(_imageDisplayDelay);
			_timeBetween = new WaitForSeconds(_imageDisplayTime);
		}

		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.CompareTag("Player"))
			{
				StartCoroutine(StartCutscene(collider.transform));
			}
		}

		public virtual IEnumerator StartCutscene(Transform player)
		{
			player.GetComponent<SheepController>().enabled = false;
			player.GetComponent<Animator>().SetBool("Walk", false);
			_cutsceneCamera.enabled = true;
			CameraControl.active.GetComponent<AudioListener>().enabled = false;
			_cutsceneCamera.GetComponent<AudioListener>().enabled = true;
			_farmer.SetActive(true);
			yield return _timeDelay;

			for(int i = 0; i < _worldspaceImages.Length; i++)
			{
				player.GetComponent<WorldspaceImage>().Image = _worldspaceImages[i];

				if(!player.GetComponent<WorldspaceImage>().enabled)
					player.GetComponent<WorldspaceImage>().enabled = true;
				
				yield return _timeBetween;
			}

			_cutsceneCamera.enabled = false;
			_cutsceneCamera.GetComponent<AudioListener>().enabled = false;
			CameraControl.active.GetComponent<AudioListener>().enabled = true;
			_farmer.SetActive(false);
			player.GetComponent<WorldspaceImage>().enabled = false;
			player.GetComponent<SheepController>().enabled = true;
		}
	}
}
