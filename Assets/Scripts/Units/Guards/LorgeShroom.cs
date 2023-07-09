using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorgeShroom : BaseGuard
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

    public override string Description() {
        return "Can only see adjacent squares.";
    }

    public override string Title() {
        return "ChonkShroom";
    }
}
