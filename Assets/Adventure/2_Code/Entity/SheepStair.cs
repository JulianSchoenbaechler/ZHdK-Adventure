using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.GameState;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider))]
	public class SheepStair : MonoBehaviour
	{
		[SerializeField] protected GameObject stair1;
		[SerializeField] protected GameObject stair2;
		[SerializeField] protected GameObject stair3;


		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.transform.CompareTag("Player"))
			{
				GameState.active = "Level3State";
			}
		}

		public void Activate(int index)
		{
			switch(index)
			{
				case 1:
					stair1.SetActive(true);
					break;

				case 2:
					stair2.SetActive(true);
					break;

				default:
					stair3.SetActive(true);
					break;
			}
		}
	}
}
