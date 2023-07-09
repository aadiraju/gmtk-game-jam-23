using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class BaseGuard : BaseUnit, Resetable
{
    [SerializeField] private int VisionDistance = 4;
    protected List<RaycastHit2D> currentHits;
	public int currentSuspicion;
	public int maxSuspicion;
	public bool sawIntruder = false;

    private bool isActive = false;

    Animator animController;
    // Start is called before the first frame update
    void Awake()
    {
        currentHits = new List<RaycastHit2D>();
        animController = GetComponent<Animator>();
		Reset();
    }

	// Update is called once per frame
    void Update()
    {

    }

    public void Reset() {
		currentSuspicion = 0;
		gameObject.SetActive(true);
		if (OccupiedTile != null && OccupiedTile.OccupyingUnit != this) {
			OccupiedTile.SetUnit(this);
		}
	}

    void FixedUpdate()
    {
        if (currentHits != null)
        {
            EraseVisionCone();
            DrawVisionCone();
        }
    }

    public override void TickUp() {
		if (gameObject.activeSelf == false) {
			return;
		}
		sawIntruder = false;
        EraseVisionCone();
        DrawVisionCone();
		if(sawIntruder) {
			currentSuspicion++;
		}
    }
    public override void Rotate(Vector2 direction)
    {
        //TODO: Make sure it's one of the 4 cardinals
        lookDirection = direction;
        animController.SetFloat("Move X", direction.x);
        animController.SetFloat("Move Y", direction.y);
    }

    protected abstract void DrawVisionCone();

    public void EraseVisionCone() {
        foreach (var hit in currentHits) {
            if (hit.collider != null) {
                Tile tile = hit.collider.GetComponent<Tile>();
                tile?.VisionUnhighlight();
            }
        }
        currentHits = new List<RaycastHit2D>();
    }

    protected RaycastHit2D[] RaycastAndHighlight(Vector2 offset, bool Circle = false, int skipDistance = 0) {
       RaycastHit2D[] hits = {};
		if (!isActive)
			return hits;
			if(Circle) {
				hits = Physics2D.CircleCastAll(transform.position + Vector3.down * 0.2f, 1, lookDirection + offset, VisionDistance, LayerMask.GetMask("Grid"));
			} else {
				hits = Physics2D.RaycastAll(transform.position + Vector3.down * 0.2f, lookDirection + offset, VisionDistance + skipDistance, LayerMask.GetMask("Grid"));
				hits = hits.Skip(skipDistance + 1).Take(VisionDistance).ToArray(); //only limit to vision distance in all directions
		}
		foreach (var hit in hits) {
			if (hit.collider != null) {
				Tile tile = hit.collider.GetComponent<Tile>();
				if (tile.isWall) {
					if (Circle) {
						continue;
					}
					break;
				}
				if (tile.OccupyingUnit != this) {
						tile?.VisionHighlight();
				}
				if (tile.OccupyingUnit is BaseIntruder) {
                    Debug.Log(this);
					sawIntruder = true;
				}
			}
		}
		return hits;
    }

    public void ToggleActive()
    {
        isActive = !isActive;
    }
}
