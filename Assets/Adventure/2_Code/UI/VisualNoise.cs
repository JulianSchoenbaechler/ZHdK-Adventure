using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualNoise : MonoBehaviour
{
	[SerializeField] protected Texture[] _images;
	[SerializeField] protected bool _randomizeImages = true;
	[SerializeField] protected Transform[] _positions;
	[SerializeField] protected bool _randomizePositions = true;
	[SerializeField][Range(0.0f, 60.0f)] protected float _delayTime = 0.5f;

	public bool PlayOneShot { get; set; }
	public bool PlayOnAwake { get; set; }
	public bool IsPlaying { get; protected set; }

	[SerializeField] private bool _playOneShot = false;
	[SerializeField] private bool _playOnAwake = false;

	private float _timer;
	private Texture _randomImage;
	private Transform _randomPos;
	private int _imageIndex, _posIndex;
	private Vector3 _screenPosition;
	private Vector2 _drawPosition;


	private void Awake()
	{
		PlayOneShot = _playOneShot;
		PlayOnAwake = _playOnAwake;	
	}

	// Use this for initialization
	protected void Start()
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


	protected void OnGUI()
	{
		if(IsPlaying)
		{
			GUI.DrawTexture(new Rect(_drawPosition.x, _drawPosition.y, _randomImage.width, _randomImage.height), _randomImage, ScaleMode.ScaleToFit);
		}
	}

	protected void Update()
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

	protected void NextImage()
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

	public void Play()
	{
		IsPlaying = true;
	}

	public void Stop()
	{
		IsPlaying = false;
		_timer = 0f;
	}
}
