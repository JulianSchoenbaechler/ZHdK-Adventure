using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.UI;
using Adventure.Interaction;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider), typeof(Animator))]
	public class OpenBarnDoor : MonoBehaviour
	{
		[SerializeField] protected AudioClip _doorClosed;
		[SerializeField] protected AudioClip _doorOpen;
		[SerializeField] protected AudioClip _successfulSound;

		protected Animator _animator;

		protected virtual void Start()
		{
			_animator = GetComponent<Animator>();

			if(GetComponent<InteractionObject>() != null)
				GetComponent<InteractionObject>().InteractionEvent += OnInteraction;
		}

		protected virtual void OnCollisionEnter(Collision collision)
		{
			if(collision.transform.CompareTag("Barrel"))
			{
				_animator.SetTrigger("Open");

				if(_doorOpen != null)
				{
					GetComponent<AudioSource>().PlayOneShot(_doorOpen);
					GameObject.FindWithTag("AmbientSound").GetComponent<AudioSource>().PlayOneShot(_successfulSound);
				}

				if(GetComponent<WorldspaceImage>() != null)
				{
					GetComponent<WorldspaceImage>().enabled = false;
				}

				if(GetComponent<InteractionObject>() != null)
				{
					GetComponent<InteractionObject>().enabled = false;
				}
			}
		}


		protected virtual void OnInteraction()
		{
			GetComponent<AudioSource>().PlayOneShot(_doorClosed);
		}
	}
}
