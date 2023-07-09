using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Videomanager : MonoBehaviour {
	private double totalTime;
    void Start () {
    	totalTime = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().clip.length;
    }
 
   
    // Update is called once per frame
    void Update () {
        double currentTime = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().time;
        if (currentTime >= totalTime * 0.99) {
			gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().time = 14f;
        }
    }
}
