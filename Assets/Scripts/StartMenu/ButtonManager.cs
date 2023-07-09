using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
	public string GameScene;
	public GameObject CreditsPopup;

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
		//SceneManager.LoadScene(GameScene, LoadSceneMode.Additive);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void OnCreditsButtonPress() {
		CreditsPopup.SetActive(true);
	}

	public void OnCreditsCloseButtonPress() {
		CreditsPopup.SetActive(false);
	}
}
