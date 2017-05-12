using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adventure.UI
{
	public class UISpray : MonoBehaviour
	{
		// Properties
		public bool IsPlaying { get; protected set; }

		// Serialized
		[SerializeField] protected GameObject[] _UIElements;
		[SerializeField] protected bool _sprayElements = true;
		[SerializeField] protected float _sprayInterval = 0.1f;
		[SerializeField] protected float _duration = 1f;
		[SerializeField] private bool _playOnAwake = false;

		private WaitForSeconds _sprayDelay;
		private WaitForSeconds _sprayDuration;
		private Coroutine _sprayRoutine;


		/// <summary>
		/// On Awake.
		/// </summary>
		protected virtual void Awake()
		{
			// Disable UI elements
			for(int i = 0; i < _UIElements.Length; i++)
			{
				_UIElements[i].SetActive(false);
			}
		}

		/// <summary>
		/// Initialization
		/// </summary>
		protected virtual void Start()
		{
			if(_playOnAwake)
			{
				Play();
			}
			else
			{
				IsPlaying = false;
			}

			_sprayDelay = new WaitForSeconds(_sprayInterval);
			_sprayDuration = new WaitForSeconds(_duration - _sprayInterval);
		}

		/// <summary>
		/// Start playing this spray.
		/// </summary>
		public virtual void Play()
		{
			IsPlaying = true;
			_sprayRoutine = StartCoroutine(Spray());
		}

		/// <summary>
		/// Stop spray.
		/// </summary>
		public virtual void Stop()
		{
			StopCoroutine(_sprayRoutine);
			Awake();
			IsPlaying = false;
		}

		/// <summary>
		/// Start spraying. Invoke as a coroutine.
		/// </summary>
		protected virtual IEnumerator Spray()
		{
			for(int i = 0; i < _UIElements.Length; i++)
			{
				_UIElements[i].SetActive(true);
				yield return _sprayDelay;
			}

			yield return _sprayDuration;
			Awake();
		}
	}
}
