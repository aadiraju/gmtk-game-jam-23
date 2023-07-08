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

}
