using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class BaseGuard : BaseUnit
{
    [SerializeField] private int VisionDistance = 4;
    protected List<RaycastHit2D> currentHits;
    bool alert = false;
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
        EraseVisionCone();
        DrawVisionCone();
		foreach (var hit in currentHits) {
			Debug.Log(hit);
		}
    }
    public override void Rotate(Vector2 direction) {
        //TODO: Make sure it's one of the 4 cardinals
        lookDirection = direction;
        animController.SetFloat("Move X", direction.x);
        animController.SetFloat("Move Y", direction.y);
    }

    protected abstract void DrawVisionCone();

    protected void EraseVisionCone() {
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

    protected RaycastHit2D[] RaycastAndHighlight(Vector2 offset, bool Circle = false) {
		RaycastHit2D[] hits = {};
		if(Circle) {
			hits = Physics2D.CircleCastAll(transform.position + Vector3.down * 0.2f, 1, lookDirection + offset, VisionDistance, LayerMask.GetMask("Grid"));
		} else {
			hits = Physics2D.RaycastAll(transform.position + Vector3.down * 0.2f, lookDirection + offset, VisionDistance, LayerMask.GetMask("Grid"));
			hits = hits.Skip(0).Take(VisionDistance).ToArray(); //only limit to vision distance in all directions
		}
		foreach (var hit in hits) {
			// if (hit.collider != null) {
			// 	Tile tile = hit.collider.GetComponent<Tile>();
			// 	if (tile.isWall) {
			// 		break;
			// 	}
			// 	if(tile.OccupyingUnit != this) {
			// 		tile?.Highlight();
			// 	}
			// 	if(tile.OccupyingUnit is BaseIntruder && alert == false){
			// 		Debug.Log("Guard has discovered intruder");
			// 		alert = true;
			// 	}
			// }
		}
		return hits;
    }
}
