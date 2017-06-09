using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.CameraHandling;

namespace Adventure.UI
{
	public class WorldspaceImage : MonoBehaviour
	{
		// Serialized
		[SerializeField] protected Camera _camera;
		[SerializeField] protected Texture _image;
		[SerializeField] protected float _scale = 1f;
		[SerializeField][Range(0f, 360f)] protected float _angle = 0f;
		[SerializeField] protected Transform _target;
		[SerializeField] protected Vector2 _pixelOffset;

		// Private
		protected Vector3 _screenPosition;
		protected Vector2 _drawPosition;
		private Matrix4x4 _matrixBackup;

		// Properties
		public Camera Camera
		{
			get { return _camera; }
			set { _camera = value; }
		}

		public Texture Image
		{
			get { return _image; }
			set { _image = value; }
		}


		/// <summary>
		/// Initialization
		/// </summary>
		protected virtual void Start()
		{
			if(_target == null)
				_target = transform;
			
			_matrixBackup = GUI.matrix;
		}

		/// <summary>
		/// On GUI.
		/// </summary>
		protected virtual void OnGUI()
		{
			if(_image != null)
			{
				GUIUtility.ScaleAroundPivot(Vector2.one * _scale, _screenPosition);
				GUIUtility.RotateAroundPivot(_angle, _drawPosition);

				GUI.DrawTexture(
					new Rect(_drawPosition.x, _drawPosition.y, (_image.width / 2f), (_image.height / 2f)),
					_image,
					ScaleMode.ScaleToFit
				);

				GUI.matrix = _matrixBackup;
			}
		}

		/// <summary>
		/// Every frame.
		/// </summary>
		protected virtual void Update()
		{
			if(_image != null)
			{
				if(_camera != null)
					_screenPosition = _camera.WorldToScreenPoint(_target.position);
				else	
					_screenPosition = CameraControl.active.WorldToScreenPoint(_target.position);
				

				_screenPosition.y = Screen.height - _screenPosition.y;
				_drawPosition.Set(
					_screenPosition.x + _pixelOffset.x,
					_screenPosition.y - _pixelOffset.y
				);
			}
		}
	}
}
