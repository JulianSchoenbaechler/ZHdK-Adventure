using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adventure.CameraHandling;

namespace Adventure.UI
{
	public class VisualNoise : MonoBehaviour
	{
		// Properties
		public bool PlayOneShot { get; set; }
		public bool PlayOnAwake { get; set; }
		public bool IsPlaying { get; protected set; }

		// Serialized
		[SerializeField] protected Texture[] _images;
		[SerializeField] protected bool _randomizeImages = true;
		[SerializeField] protected Transform[] _positions;
		[SerializeField] protected bool _randomizePositions = true;
		[SerializeField][Range(0.0f, 60.0f)] protected float _delayTime = 0.5f;

		[SerializeField] private bool _playOneShot = false;
		[SerializeField] private bool _playOnAwake = false;

		// Private
		private float _timer;
		protected Texture _randomImage;
		protected Transform _randomPos;
		protected int _imageIndex, _posIndex;
		protected Vector3 _screenPosition;
		protected Vector2 _drawPosition;


		/// <summary>
		/// On awake.
		/// </summary>
		private void Awake()
		{
			PlayOneShot = _playOneShot;
			PlayOnAwake = _playOnAwake;	
		}

		/// <summary>
		/// Initialization
		/// </summary>
		protected virtual void Start()
		{
			if(PlayOnAwake)
			{
				NextImage();
				IsPlaying = true;
			}
			else
			{
				IsPlaying = false;
			}
		}

		/// <summary>
		/// On GUI.
		/// </summary>
		protected virtual void OnGUI()
		{
			if(IsPlaying)
			{
				GUI.DrawTexture(new Rect(_drawPosition.x, _drawPosition.y, _randomImage.width, _randomImage.height), _randomImage, ScaleMode.ScaleToFit);
			}
		}

		/// <summary>
		/// Every frame.
		/// </summary>
		protected virtual void Update()
		{
			if(IsPlaying)
			{
				if(_timer >= _delayTime)
				{
					if(PlayOneShot)
					{
						IsPlaying = false;
					}

					NextImage();

					_timer = 0f;
				}
				else
				{
					_timer += Time.deltaTime;
				}
			}
		}

		/// <summary>
		/// Preloads the next image to display. Calculations screen position.
		/// </summary>
		protected virtual void NextImage()
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

			_screenPosition = CameraControl.active.WorldToScreenPoint(_randomPos.position);
			_drawPosition = new Vector2(
				_screenPosition.x - (_randomImage.width / 2f),
				Screen.height - (_screenPosition.y + (_randomImage.height / 2f))
			);
		}

		/// <summary>
		/// Start playing this noise.
		/// </summary>
		public virtual void Play()
		{
			IsPlaying = true;
		}

		/// <summary>
		/// Stop noise.
		/// </summary>
		public virtual void Stop()
		{
			IsPlaying = false;
			_timer = 0f;
		}
	}
}
