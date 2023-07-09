using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowShroom : BaseGuard
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
        currentHits.AddRange(RaycastAndHighlight(Vector2.up, true));
    }

    public override string SelectedString() {
        return "GlowShroom. Can only see adjacent tiles.";
    }
}
