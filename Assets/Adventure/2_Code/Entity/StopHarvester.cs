using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider))]
	public class StopHarvester : MonoBehaviour
	{
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.CompareTag("Harvester"))
			{
				Animator animator = collider.transform.parent.GetComponent<Animator>();
				animator.applyRootMotion = false;
				animator.speed = 0.5f;
			}
		}
	}
}
