using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject _mainPanel;
	[SerializeField] private GameObject _howToPanel;

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void LoadCredits()
	{
		SceneManager.LoadScene(2);
	}

	public void ShowHowTo()
	{
		_howToPanel.SetActive(true);
		_mainPanel.SetActive(false);
	}

	public void ShowMain()
	{
		_mainPanel.SetActive(true);
		_howToPanel.SetActive(false);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
