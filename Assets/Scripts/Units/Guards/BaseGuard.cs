using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGuard : BaseUnit
{
    [SerializeField] private int VisionDistance = 4;
    private Vector2 offset1, offset2;
    private List<RaycastHit2D> currentHits;
    
    Animator animController;
    // Start is called before the first frame update
    void Awake()
    {
        currentHits = new List<RaycastHit2D>();
        animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        if(currentHits != null) {
            EraseVisionCone();
            DrawVisionCone();
        }
    }

    public override void TickUp() {
        
    }
    public override void Rotate(Vector2 direction) {
        //TODO: Make sure it's one of the 4 cardinals
        lookDirection = direction;
        animController.SetFloat("Move X", direction.x);
        animController.SetFloat("Move Y", direction.y);
    }

    void DrawVisionCone() {
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

    void EraseVisionCone() {
        foreach (var hit in currentHits) {
            if (hit.collider != null) {
                Tile tile = hit.collider.GetComponent<Tile>();
                if(tile.OccupyingUnit != this) {
                    tile?.Unhighlight();
                }
            }
        }
        currentHits = new List<RaycastHit2D>();
    }

    RaycastHit2D[] RaycastAndHighlight(Vector2 offset) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.down * 0.2f, lookDirection + offset, VisionDistance, LayerMask.GetMask("Grid"));
         foreach (var hit in hits) {
            if (hit.collider != null) {
                Tile tile = hit.collider.GetComponent<Tile>();
                if(tile.OccupyingUnit != this) {
                    tile?.Highlight();
                }
            }
        }
        return hits;
    }
}
