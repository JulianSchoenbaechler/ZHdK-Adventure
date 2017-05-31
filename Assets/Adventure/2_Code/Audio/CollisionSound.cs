using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JulianSchoenbaechler.SnapPanel;

namespace Adventure.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class CollisionSound : MonoBehaviour
	{
		[SerializeField] protected AudioClip[] _clips;
		[SerializeField] protected float _pitchRandomization = 0f;
		[SerializeField] protected bool _maskWithTag = false;
		[SerializeField] protected string _tag;
		[SerializeField] protected bool _velocityCheck = false;
		[SerializeField] protected float _minVelocity = 0f;

		protected AudioSource _audioSource;


		protected virtual void Start()
		{
			_audioSource = GetComponent<AudioSource>();
		}

		protected virtual void OnCollisionEnter(Collision collisionInfo)
		{
			if(_maskWithTag && collisionInfo.transform.CompareTag(_tag))
			{
				_audioSource.PlayRandomizedShot(_pitchRandomization, _clips);
			}
			else if(_velocityCheck && (collisionInfo.relativeVelocity.magnitude >= _minVelocity))
			{
				_audioSource.PlayRandomizedShot(_pitchRandomization, _clips);
			}
			else if(!_maskWithTag && !_velocityCheck)
			{
				_audioSource.PlayRandomizedShot(_pitchRandomization, _clips);
			}
		}
	}
}
