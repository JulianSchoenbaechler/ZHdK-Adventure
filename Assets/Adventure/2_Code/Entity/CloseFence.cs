using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider), typeof(Animator))]
	public class CloseFence : MonoBehaviour
	{
		protected Animator _animator;


		protected virtual void Start()
		{
			_animator = GetComponent<Animator>();
			_animator.enabled = false;
		}

		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.transform.CompareTag("Player"))
			{
				_animator.enabled = true;
			}
		}
	}
}
