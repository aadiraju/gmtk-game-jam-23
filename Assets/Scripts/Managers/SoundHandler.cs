using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    private AudioSource[] MySounds;
    private AudioSource Click;
    private AudioSource Detect;
    private AudioSource PlanningMusic;
    private AudioSource SimulationMusic;
	private AudioSource Victory;
	private AudioSource Loss;

    void Start() {
        MySounds = GetComponents<AudioSource>();
        PlanningMusic = MySounds[0];
        SimulationMusic = MySounds[1];
        Click = MySounds[2];
        Detect = MySounds[3];
		Victory = MySounds[4];
		Loss = MySounds[5];
    }

    public void PlayPlanningMusic() {
        PlanningMusic.Play();
    }

    public void StopPlanningMusic() {
        PlanningMusic.Stop();
    }

    public void PlaySimulationMusic() {
        SimulationMusic.Play();
    }

    public void StopSimulationMusic() {
        SimulationMusic.Stop();
    }

    public void PlayClick() {
        Click.Play();
    }

    public void PlayDetect() {
        Detect.Play();
    }

	public void PlayVictory() {
		Victory.Play();
	}

	public void PlayLoss() {
		Loss.Play();
	}
}
