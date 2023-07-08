using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinShroom : BaseGuard
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void DrawVisionCone() {
        Vector2 DirectionA;
        Vector2 DirectionB;
        if(this.lookDirection == Vector2.up || this.lookDirection == Vector2.down) {
            DirectionA = Vector2.left;
            DirectionB = Vector2.right;
        } else {
            DirectionA = Vector2.up;
            DirectionB = Vector2.down;
        }

        float[] offsets = {0.25f, 0.5f};
        Vector2[] lookDirectionMods = {Vector2.zero, lookDirection * -2};
        
        foreach (var lookDirectionMod in lookDirectionMods)
        {
            currentHits.AddRange(RaycastAndHighlight(lookDirectionMod + Vector2.zero));
            foreach (var offset in offsets)
            {
                currentHits.AddRange(RaycastAndHighlight(lookDirectionMod + DirectionA * offset));
                currentHits.AddRange(RaycastAndHighlight(lookDirectionMod + DirectionB * offset));
            }   
        }
    }
}
