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
		protected Animator _animator;


		protected virtual void Start()
		{
			_animator = GetComponent<Animator>();
		}

		protected virtual void OnCollisionEnter(Collision collision)
		{
			if(collision.transform.CompareTag("Barrel"))
			{
				_animator.SetTrigger("Open");

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
	}
}
