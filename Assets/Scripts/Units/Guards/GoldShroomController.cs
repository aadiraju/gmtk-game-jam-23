using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldShroomController : BaseGuard
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

    }

    void FixedUpdate() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + Vector3.down * 0.2f, 1f, Vector2.zero, 1f, LayerMask.GetMask("Grid"));
         foreach (var hit in hits) {
            if (hit.collider != null) {
                Tile tile = hit.collider.GetComponent<Tile>();
                if(tile.OccupyingUnit is BaseIntruder) {
                   Debug.Log("He won, you lost boohoo .·´¯`(>▂<)´¯`·. ");
                }
            }
        }
    }
}
