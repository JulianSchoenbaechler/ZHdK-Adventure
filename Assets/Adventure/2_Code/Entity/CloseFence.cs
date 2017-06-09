using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.GameState;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider), typeof(Animator))]
	public class CloseFence : MonoBehaviour
	{
		[SerializeField] protected GameObject _farmerShoot;

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
				_farmerShoot.SetActive(true);
				GetComponent<AudioSource>().PlayDelayed(0.8f);
				GameState.active = "Level2State";
			}
		}
	}
}
