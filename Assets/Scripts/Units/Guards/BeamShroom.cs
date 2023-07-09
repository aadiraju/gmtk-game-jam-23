using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamShroom : BaseGuard
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
        currentHits.AddRange(RaycastAndHighlight(Vector2.zero));
    }

    public override string Description() {
        return "Can see very far forward in a narrow direction.";
    }

    public override string Title() {
        return "BeamShroom";
    }
}
