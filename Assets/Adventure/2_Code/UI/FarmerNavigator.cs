using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.CameraHandling;

namespace Adventure.UI
{
	public class FarmerNavigator : MonoBehaviour
	{
		[SerializeField] protected Texture _indicator;
		[SerializeField] protected Transform _farmer;

		private Vector3 _screenPoint;
		private Vector2 _indicatorPoint;
		private Vector2 _screenCenter;
		private Matrix4x4 _matrixBackup;
		private float _indicatorAngle;

		// Use this for initialization
		void Start()
		{
			_screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
			_indicatorPoint = new Vector2();
			_matrixBackup = GUI.matrix;
		}
		
		// Update is called once per frame
		void LateUpdate()
		{
			// Get pixel-correct point on screen from active camera
			// Flip y-axis in order to match screen coordinates.
			_screenPoint = CameraControl.active.WorldToScreenPoint(_farmer.position);
			_screenPoint.y = Screen.height - _screenPoint.y;

			// Is offscreen?
			if((_screenPoint.x < 0f) || (_screenPoint.x > Screen.width) ||
			   (_screenPoint.y < 0f) || (_screenPoint.y > Screen.height) ||
			   (_screenPoint.z < 0f))
			{
				// In behind of object
				if(_screenPoint.z < 0f)
				{
					// All coordinates are flipped
					_screenPoint.x = Screen.width - _screenPoint.x;
					_screenPoint.y = Screen.height - _screenPoint.y;
				}


				_indicatorPoint.Set(_screenPoint.x, _screenPoint.y);

				// Move zero point into the center of the screen
				// (0/0) from bottom left to center
				_indicatorPoint += _screenCenter;
				//_indicatorPoint.Normalize();

				_indicatorAngle = Mathf.Atan2(_screenPoint.y, _screenPoint.x);
				_indicatorAngle -= CameraControl.active.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
				//print(_indicatorAngle * Mathf.Rad2Deg);

				float cos = Mathf.Cos(_indicatorAngle);
				float sin = Mathf.Sin(_indicatorAngle);
				_indicatorPoint = _screenCenter + new Vector2(sin * 150f, cos * 150f);
				/*
				if(Mathf.Sin(_indicatorAngle) > 0f)
				{
					_indicatorPoint *= _screenCenter.y / Mathf.Cos(_indicatorAngle - (90f * Mathf.Deg2Rad));
				}
				else
				{
					_indicatorPoint *= _screenCenter.x / Mathf.Cos(_indicatorAngle - (90f * Mathf.Deg2Rad));
					print(_screenCenter.x / Mathf.Cos(_indicatorAngle - (90f * Mathf.Deg2Rad)));
				}
				*/
				

				//angle = angle * Mathf.Rad2Deg;
				//print(Mathf.Cos(angle));
				//_indicatorPoint = _indicatorPoint.normalized * _screenCenter.magnitude;

				// y = mx + b
			}
		}

		/// <summary>
		/// On GUI.
		/// </summary>
		protected virtual void OnGUI()
		{
			Drawing.DrawLine(_screenCenter, _indicatorPoint, Color.red);
			/*
			GUIUtility.RotateAroundPivot(_indicatorAngle * Mathf.Rad2Deg - 90f, _screenCenter);
			GUI.DrawTexture(new Rect(_screenCenter.x - (_indicator.width / 2f) , _screenCenter.y - (_indicator.height / 2f), _indicator.width, _indicator.height), _indicator, ScaleMode.ScaleToFit);
			GUI.matrix = _matrixBackup;
			*/
		}
	}
}
