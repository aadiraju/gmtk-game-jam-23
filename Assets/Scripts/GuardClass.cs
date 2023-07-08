using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardClass {
    // Current turn information
    Vector2 Direction  {get; set;}
    private bool IsActive {get; set;}

    private int CurrentSuspicion {get; set;}
    private int MaxSuspicion {get; set;}

    private int CurrentEyeOpen {get; set;}
    private int MaxEyeOpen {get; set;}
    private int CurrentEyeClosed {get; set;}
    private int MaxEyeClosed {get; set;}
    private bool EyeClosed {get; set;}

    private bool RotateClockWise {get; set;}
    private int CurrentRotationCount {get; set;}
    private int MaxRotationCount {get; set;}

    private int[,] VisionCone;
    private int[,] UpVisionCone {get;} 
    private int[,] RightVisionCone {get;}
    private int[,] DownVisionCone {get;}
    private int[,] LeftVisionCone {get;}
    /* 
    This attribute should be a 11 by 11 matrix. The center of the matrix
    is where the guard is centered, the matrix orientation is assuming the guard is
    looking directly upwards. Example matrix for a stationary mushroom:
    1 1 1 1 1 1 1 1 1 1 1
    0 1 1 1 1 1 1 1 1 1 0
    0 0 1 1 1 1 1 1 1 0 0
    0 0 0 1 1 1 1 1 0 0 0
    0 0 0 0 1 1 1 0 0 0 0
    0 0 0 0 0 X 0 0 0 0 0
    0 0 0 0 0 0 0 0 0 0 0
    0 0 0 0 0 0 0 0 0 0 0
    0 0 0 0 0 0 0 0 0 0 0
    0 0 0 0 0 0 0 0 0 0 0
    0 0 0 0 0 0 0 0 0 0 0
    
    0 - not looking at that location, 
    1 - visible
    x - where they mushroom is located (should be 0 in actual matrix)
    */

    private void CreateAllMatrix(){
        // Create a clone of the original vision cone: 
        // Set that clone to be the upcone
        // Create a clone of the original vision cone
        // Rotate it clockwise 90 degrees
        // Set that to be the right matrix
        // create mirrored clone of up matrix: that is down matrix
        // Create mirrored clone of right matrix: that is left matrix
    }

    public void EveryTick () {
        if (!IsActive) {
            return;
        }
        if (!EyeClosed) {
            CurrentEyeOpen += 1;
            if (CurrentEyeOpen == MaxEyeOpen) {
                EyeClosed = true;
                CurrentEyeOpen = 0;
            } 
        } else {
            CurrentEyeClosed += 1;
            if (CurrentEyeClosed == MaxEyeClosed) {
                EyeClosed = false;
                CurrentEyeClosed = 0;
            }
        }
        CurrentRotationCount += 1;
        if (CurrentRotationCount == MaxRotationCount){
            CurrentRotationCount = 0;

            if(RotateClockWise) {
                if(Direction == Vector2.up) {
                    Direction = Vector2.right;
                } else if(Direction == Vector2.right) {
                    Direction = Vector2.down;
                } else if(Direction == Vector2.down) {
                    Direction = Vector2.left;
                } else {
                    Direction = Vector2.up;
                }
            } else {
                if(Direction == Vector2.up) {
                    Direction = Vector2.left;
                } else if(Direction == Vector2.left) {
                    Direction = Vector2.down;
                } else if(Direction == Vector2.down) {
                    Direction = Vector2.right;
                } else {
                    Direction = Vector2.up;
                }
            }
        }
    }

    public RaiseSuspicion() {
        CurrentSuspicion += 1;
    }

    public LowerSuspicion() {
        CurrentSuspicion -= 1;
    }
    
}
