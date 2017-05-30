﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.SnapPanel;

namespace Adventure.Entity
{
	[RequireComponent(typeof(Collider))]
	public class CornDestroy : MonoBehaviour
	{
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.transform.CompareTag("Harvester"))
			{
				GameObject.Destroy(gameObject);
			}
		}
	}
}
