using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Adventure.Entity
{
	public class WinGame : MonoBehaviour
	{
		[SerializeField] protected Image _white;
		[SerializeField] protected GameObject _displayUI;
		[SerializeField] protected float _delay;

		private bool _win = false;
		private Color _startColor;
		private float _colorLerp = 0f;


		protected void Start()
		{
			_startColor = new Color(1f, 1f, 1f, 0f);
		}

		protected void Update()
		{
			if(_win)
			{
				if(!_white.gameObject.activeInHierarchy)
					_white.gameObject.SetActive(true);

				_white.color = Color.Lerp(_startColor, Color.white, _colorLerp);
				_colorLerp = _colorLerp + (Time.unscaledDeltaTime * 0.3f);
			}

			if(_win && Input.GetKeyDown(KeyCode.Space))
			{
				SceneManager.LoadScene(0);
			}
		}

		protected virtual void OnTriggerEnter(Collider collider)
		{
			if(collider.transform.CompareTag("Player"))
			{
				_win = true;
				Invoke("DisplayUI", _delay);
			}
		}

		private void DisplayUI()
		{
			_displayUI.SetActive(true);
		}
	}
}
