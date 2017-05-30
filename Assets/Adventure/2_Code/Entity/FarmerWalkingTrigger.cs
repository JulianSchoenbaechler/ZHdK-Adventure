using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider))]
	public class FarmerWalkingTrigger : MonoBehaviour
	{
		[SerializeField] protected GameObject _farmer;

		//private bool _doorOpen = false;

		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.CompareTag("Player"))
			{
				//if(_doorOpen)
					_farmer.SetActive(true);
			}/*
			else if(collider.CompareTag("Barrel"))
			{
				_doorOpen = true;
			}
			*/
		}
	}
}
