﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.CameraHandling;

namespace Adventure.UI
{
	public class FarmerNavigator : MonoBehaviour
	{
		[SerializeField] protected Texture _indicatorTop;
		[SerializeField] protected Texture _indicatorBottom;
		[SerializeField] protected Texture _indicatorLeft;
		[SerializeField] protected Texture _indicatorRight;
		[SerializeField] protected Transform _farmer;

		protected SkinnedMeshRenderer _renderer;

		private Texture _indicator;					// Displayed farmer indicator
		private Vector3 _yMask;						// Vector mask (1,0,1)
		private Vector3 _distanceVector;			// Vector from farmer to current camera
		private Vector3 _cameraVector;				// Current camera forward vector
		private Vector2 _indicatorPoint;			// Position for the screen indicator in screen space
		private Vector2 _screenCenter;				// Screen center
		private float _indicatorAngle;				// Calculated indicator angle in screen space (cosinus format)
		private float _dampedIndicatorAngle;		// Damped indicator angle (0...1)
		private float _clippingAngleCos;			// Angle indicating the crossing vector from screen center to screen edge

		/// <summary>
		/// Initialization.
		/// </summary>
		void Start()
		{
			_renderer = _farmer.GetComponentInChildren<SkinnedMeshRenderer>();

			// Vector2 loaded with values half the screen size
			_screenCenter = new Vector2(Screen.width, Screen.height) / 2f;

			// Cosinus of angle which defines the screen edges.
			/*        front
			 *          1
			 * left 0       0 right
			 *         -1
			 *        back
			 */
			_clippingAngleCos = Mathf.Cos((90f * Mathf.Deg2Rad) - Mathf.Atan2(_screenCenter.y, _screenCenter.x));
			_indicatorPoint = new Vector2();
			_yMask = new Vector3(1f, 0f, 1f);
		}


		/// <summary>
		/// After every frame.
		/// </summary>
		void LateUpdate()
		{
			// Calculation only if farmer is visible
			if(!IsVisibleFrom(_renderer, CameraControl.active))
			{
				// Vector from farmer to currently active camera.
				_distanceVector = _farmer.position - CameraControl.active.transform.position;
				_distanceVector.Scale(_yMask);

				_cameraVector = Vector3.Scale(CameraControl.active.transform.forward, _yMask);

				// Calculate angle between the camera forward vector and the distance vector
				/*        front
				 *          1
				 * left 0       0 right
				 *         -1
				 *        back
				 */
				_indicatorAngle = Vector3.Dot(CameraControl.active.transform.forward, _distanceVector.normalized);

				// Damping angle with polynom-function
				_dampedIndicatorAngle = DampAngle(Mathf.Abs(_indicatorAngle));

				// If the cross product of the camera forward vector and the distance vector returns a vector pointing upwards,
				// the farmer is closer to the right screen side.
				if(Vector3.Cross(_cameraVector.normalized, _distanceVector.normalized).y > 0f)
				{
					// Screen right

					// Horizontal or vertical?
					if(_dampedIndicatorAngle > _clippingAngleCos)
					{
						// Horizontal
						_indicatorPoint.Set(
							(_screenCenter.x / (1f - _clippingAngleCos)) * (1f - _dampedIndicatorAngle),
							_indicatorAngle > 0f ? -_screenCenter.y : _screenCenter.y
						);

						// Use top or bottom indicator
						_indicator = _indicatorAngle > 0f ? _indicatorTop : _indicatorBottom;
					}
					else
					{
						// Vertical
						_indicatorPoint.Set(
							_screenCenter.x,
							((_screenCenter.y / _clippingAngleCos) * _dampedIndicatorAngle) * (_indicatorAngle > 0f ? -1f : 1f)
						);

						// Use right indicator
						_indicator = _indicatorRight;
					}
				}
				else
				{
					// Screen left

					// Horizontal or vertical?
					if(_dampedIndicatorAngle > _clippingAngleCos)
					{
						// Horizontal
						_indicatorPoint.Set(
							-(_screenCenter.x / (1f - _clippingAngleCos)) * (1f - _dampedIndicatorAngle),
							_indicatorAngle > 0f ? -_screenCenter.y : _screenCenter.y
						);

						// Use top or bottom indicator
						_indicator = _indicatorAngle > 0f ? _indicatorTop : _indicatorBottom;
					}
					else
					{
						// Vertical
						_indicatorPoint.Set(
							-_screenCenter.x,
							((_screenCenter.y / _clippingAngleCos) * _dampedIndicatorAngle) * (_indicatorAngle > 0f ? -1f : 1f)
						);

						// Use left indicator
						_indicator = _indicatorLeft;
					}
				
				}

				// Basic calculations with minor differences
				// (width / maxAngle) * currentAngle

				// Move coordinate zero point (0/0) to the middle of the screen
				_indicatorPoint += _screenCenter;
			}
		}

		/// <summary>
		/// On GUI.
		/// </summary>
		protected virtual void OnGUI()
		{
			if(!IsVisibleFrom(_renderer, CameraControl.active))
			{
				GUI.DrawTexture(new Rect(_indicatorPoint.x - (_indicator.width / 2f), _indicatorPoint.y - (_indicator.height / 2f), _indicator.width, _indicator.height), _indicator, ScaleMode.ScaleToFit);
			}
		}
		
		/// <summary>
		/// Processes a linear function through a polygon curve in order to create a damping effect.
		/// Inversed ease-in and ease-out.
		/// </summary>
		/// <returns>The damped value.</returns>
		/// <param name="t">Value from 0 to 1.</param>
		protected virtual float DampAngle(float t)
		{
			t = Mathf.Clamp01(t);
			const float p1 = 35f / 12f;
			const float p2 = -35f / 8f;
			const float p3 = 59f / 24f;

			// p1 * x^3 + p2 * x^2 + p3 * x
			return p1 * Mathf.Pow(t, 3f) + p2 * Mathf.Pow(t, 2f) + p3 * t;
		}

		/// <summary>
		/// Determines if renderer is visible from the specified camera.
		/// </summary>
		/// <returns><c>true</c> if is visible from the specified camera; otherwise, <c>false</c>.</returns>
		/// <param name="renderer">Renderer.</param>
		/// <param name="camera">Camera.</param>
		protected bool IsVisibleFrom(Renderer renderer, Camera camera)
		{
			Vector3 temp = camera.WorldToViewportPoint(renderer.transform.position);
			Vector3 rayDir = (renderer.transform.position + Vector3.up * 0.5f) - camera.transform.position;

			if((temp.x >= 0) && (temp.x <= 1) &&
				(temp.y >= 0) && (temp.y <= 1) &&
				(temp.z >= 0))
			{
				if(!Physics.Raycast(camera.transform.position, rayDir, rayDir.magnitude  - 3f))
				{
					return true;
				}
			}

			return false;
		}
	}
}
