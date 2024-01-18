using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour // this button just reloads the scene
{
	public Button restartButton;

	void Start()
	{
		Button btn = restartButton.GetComponent<Button>();
		btn.onClick.AddListener(ButtonClick);
	}

	void ButtonClick()
	{
		Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
	}
}
