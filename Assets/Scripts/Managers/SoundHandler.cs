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

    void Start() {
        MySounds = GetComponents<AudioSource>();
        Debug.Log(MySounds.Length);
        PlanningMusic = MySounds[0];
        SimulationMusic = MySounds[1];
        Click = MySounds[2];
        Detect = MySounds[3];
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

}
