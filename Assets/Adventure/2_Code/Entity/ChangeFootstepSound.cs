using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.Character;

namespace Adventure.Entity
{
	public class ChangeFootstepSound : MonoBehaviour
	{
		[SerializeField] protected bool _inBarn = true;


		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.CompareTag("Player"))
			{
				collider.GetComponent<SheepController>().InBarnFootsteps = _inBarn;
			}
		}
	}
}
