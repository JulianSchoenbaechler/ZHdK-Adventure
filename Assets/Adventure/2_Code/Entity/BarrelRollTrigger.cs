using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.SnapPanel;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider))]
	public class BarrelRollTrigger : MonoBehaviour
	{
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.transform.CompareTag("Barrel"))
			{
				GetComponentInChildren<SnapPanel>().StartSnapping();
			}
		}
	}
}
