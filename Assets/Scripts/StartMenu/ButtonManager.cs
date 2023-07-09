using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
	public string GameScene;
	private MainMenuMusicManager MusicManager;
	public GameObject CreditsPopup;
	public List<GameObject> Buttons;

	void Awake() {
		MusicManager = FindObjectOfType<MainMenuMusicManager>();

		// Wait 11 seconds, then show buttons
		Invoke("ShowButtons", 11f);
	}

	public void OnStartButtonPress() {
		// Remove old AudioListener
		AudioListener[] aL = FindObjectsOfType<AudioListener>();
		for (int i = 0; i < aL.Length; i++)
		{
			//Ignore the first AudioListener in the array 
			if (i == 0)
				continue;

			//Destroy 
			DestroyImmediate(aL[i]);
		}
		SceneManager.LoadScene(GameScene, LoadSceneMode.Additive);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void OnCreditsButtonPress() {
		CreditsPopup.SetActive(true);

		MusicManager.StopMainMenuMusic();
		MusicManager.PlayCreditsMusic();
	}

	public void OnCreditsCloseButtonPress() {
		CreditsPopup.SetActive(false);

		MusicManager.StopCreditsMusic();
		MusicManager.PlayMainMenuMusic();
	}

	public void ShowButtons() {
		foreach (var button in Buttons) {
			button.SetActive(true);
		}
	}

	public void HideButtons() {
		foreach (var button in Buttons) {
			button.SetActive(false);
		}
	}
}
