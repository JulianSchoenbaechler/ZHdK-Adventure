using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Adventure.Audio;

public class CreditsWalk : MonoBehaviour
{
	[SerializeField] protected Image _whiteScreen;
	[SerializeField] protected float _blendSpeed = 0.5f;
	[SerializeField] protected float _speed = 0.5f;
	[SerializeField] protected AudioClip[] _footsteps;
	[SerializeField] protected float _footstepDelay = 0.5f;

	protected Color _finalColor;
	protected float _colorLerp = 0f;
	protected float _footstepTime = 0f;


	public void ExitScreen()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	protected void Start()
	{
		_finalColor = new Color(1f, 1f, 1f, 0f);
		GetComponent<Animator>().speed = 0.8f;
		GetComponent<Animator>().SetBool("Walk", true);
	}

	protected void Update()
	{
		// Footstep play
		if(_footstepTime >= _footstepDelay)
		{
			GetComponent<AudioSource>().PlayRandomizedShot(0.1f, _footsteps);
			_footstepTime = 0f;
		}
		else
		{
			_footstepTime += Time.deltaTime;
		}

		if(_whiteScreen.IsActive())
		{
			_whiteScreen.color = Color.Lerp(Color.white, _finalColor, _colorLerp);
			_colorLerp += Time.deltaTime * _blendSpeed;

			if(_colorLerp >= 1f)
				_whiteScreen.gameObject.SetActive(false);
		}
	}

	protected void FixedUpdate()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * _speed;
	}
}
