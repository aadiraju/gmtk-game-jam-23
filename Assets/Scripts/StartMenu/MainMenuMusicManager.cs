using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicManager : MonoBehaviour {
	private AudioSource[] MySounds;
	private AudioSource MainMenuMusic;
	private AudioSource CreditsMusic;
	
    void Start() {
		MySounds = GetComponents<AudioSource>();
        MainMenuMusic = MySounds[0];
		CreditsMusic = MySounds[1];
    }

	public void PlayMainMenuMusic() {
		MainMenuMusic.Play();
	}

	public void StopMainMenuMusic() {
		MainMenuMusic.Stop();
	}

	public void PlayCreditsMusic() {
		CreditsMusic.Play();
	}

	public void StopCreditsMusic() {
		CreditsMusic.Stop();
	}
}
