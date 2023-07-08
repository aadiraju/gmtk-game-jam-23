using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGuard : BaseUnit
{
    [SerializeField] private int VisionDistance = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        RaycastAndHighlight(Vector2.zero);
        RaycastAndHighlight(Vector2.left * 0.125f);
        RaycastAndHighlight(Vector2.left * 0.25f);
        RaycastAndHighlight(Vector2.left * 0.5f);
        RaycastAndHighlight(Vector2.right * 0.125f);
        RaycastAndHighlight(Vector2.right * 0.25f);
        RaycastAndHighlight(Vector2.right * 0.5f);
    }

    void RaycastAndHighlight(Vector2 offset) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.down * 0.2f, lookDirection + offset, VisionDistance, LayerMask.GetMask("Grid"));
         foreach (var hit in hits) {
            if (hit.collider != null) {
                Tile tile = hit.collider.GetComponent<Tile>();
                if(tile.OccupyingUnit != this) {
                    tile?.Highlight();
                }
            }
        }
    }
}
