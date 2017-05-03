using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using SnapPanel
using JulianSchoenbaechler.SnapPanel;


public class Cube : MonoBehaviour
{
	public SnapPanel controller;
	
	// Update is called once per frame
	void Update()
	{
		// Press <Space> to start SnapPanel
		if(Input.GetKeyDown(KeyCode.Space))
		{
			controller.StartSnapping();
		}


		// Animation just for demonstration...
		transform.Rotate(0f, 100f * Time.deltaTime, 0f);
	}
}
