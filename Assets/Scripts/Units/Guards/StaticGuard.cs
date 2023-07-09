using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticGuard : BaseGuard
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

        float[] offsets = {0.125f, 0.25f, 0.5f};
        
        currentHits.AddRange(RaycastAndHighlight(Vector2.zero));
        foreach (var offset in offsets)
        {
               currentHits.AddRange(RaycastAndHighlight(DirectionA * offset));
               currentHits.AddRange(RaycastAndHighlight(DirectionB * offset));
        }
    }

    public override string Description() {
        return "Looks forward for medium distance.";
    }

    public override string Title() {
        return "StaticShroom";
    }
}
