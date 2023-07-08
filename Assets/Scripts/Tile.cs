using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour {
	[SerializeField] private Sprite[] tileSprites;
    [SerializeField] private Color normalColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;
    
    [SerializeField] private bool isWalkable = true;
    [SerializeField] private int LightLevel;

    public BaseUnit OccupyingUnit = null;
    public bool Walkable => isWalkable && OccupyingUnit == null;

    public void Init(bool isOffset) {
        renderer.color = isOffset ? offsetColor :  normalColor;
        isWalkable = true;
    }

	public void Init(Variant variant) {
		if (variant == Variant.empty) {
			variant = Variant.pathEmpty;
		}
		renderer.sprite = tileSprites[(int) variant];
		if ((int) variant >= 13 && (int) variant <= 18) {
			isWalkable = true;
		} else {
			isWalkable = false;
		}
	}

    void OnMouseEnter() {
        highlight.SetActive(true);
    }

    void OnMouseExit() {
        highlight.SetActive(false);
    }

    public void SetUnit(BaseUnit baseUnit) {
        if(baseUnit.OccupiedTile != null) {
            baseUnit.OccupiedTile.OccupyingUnit = null;
        }
        baseUnit.transform.position = transform.position;
        OccupyingUnit = baseUnit;
        baseUnit.OccupiedTile = this;
    }

	public enum Variant {
		wallCorner0,
		wallCorner1,
		wallCorner2,
		wallCorner3,
		wallCorner4,
		wallCorner5,
		wallCorner6,
		wallCorner7,
		wallEdge0,
		wallEdge1,
		wallEdge2,
		wallEdge3,
		wallCenter,
		pathEmpty,
		pathDirt,
		pathGrass,
		pathThickGrass,
		pathStoney,
		pathMushrooms,
		largeStone,
		stump,
		empty
	}
}
