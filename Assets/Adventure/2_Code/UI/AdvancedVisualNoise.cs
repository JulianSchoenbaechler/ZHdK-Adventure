using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.CameraHandling;

namespace Adventure.UI
{
	public class AdvancedVisualNoise : VisualNoise
	{
		[SerializeField]protected float _scale = 1f;
		[SerializeField] protected float _randomDistance = 0f;
		[SerializeField][Range(0f, 360f)] protected float _maxRandomRotation = 0f;

		protected Vector3 _randomVector;
		private Matrix4x4 _matrixBackup;
		private float _randomRotationAngle;

		protected override void Start()
		{
			base.Start();

			_randomVector = new Vector3();
			_matrixBackup = GUI.matrix;
		}


		protected void RandomizePositionVector()
		{
			_randomVector.Set(
				Random.Range(-1f, 1f),
				Random.Range(-1f, 1f),
				Random.Range(-1f, 1f)
			);
			_randomVector.Normalize();
		}


		protected override void OnGUI()
		{
			if(IsPlaying)
			{
				GUIUtility.ScaleAroundPivot(Vector2.one * _scale, _screenPosition);
				GUIUtility.RotateAroundPivot(_randomRotationAngle, _screenPosition);
				GUI.DrawTexture(new Rect(_drawPosition.x, _drawPosition.y, _randomImage.width, _randomImage.height), _randomImage, ScaleMode.ScaleToFit);
				GUI.matrix = _matrixBackup;
			}
		}


		protected override void NextImage()
		{
			if(_randomizeImages)
			{
				_randomImage = _images[Random.Range(0, _images.Length)];
			}
			else
			{
				_randomImage = _images[_imageIndex];
				_imageIndex = (_imageIndex + 1) >= _images.Length ? 0 : (_imageIndex + 1);
			}

			if(_randomizePositions)
			{
				_randomPos = _positions[Random.Range(0, _positions.Length)];
			}
			else
			{
				_randomPos = _positions[_posIndex];
				_posIndex = (_posIndex + 1) >= _positions.Length ? 0 : (_posIndex + 1);
			}

			RandomizePositionVector();

			_screenPosition = CameraControl.active.WorldToScreenPoint(_randomPos.position + (_randomVector * _randomDistance));
			/* TODO: Not instantiate Vector2 object every time. Reduce garbage. */
			_drawPosition = new Vector2(
				_screenPosition.x - (_randomImage.width / 2f),
				Screen.height - (_screenPosition.y + (_randomImage.height / 2f))
			);

			if(_maxRandomRotation > 0f)
				_randomRotationAngle = Random.Range(_maxRandomRotation / -2f, _maxRandomRotation / 2f);
			else
				_randomRotationAngle = 0f;
		}
	}
}
