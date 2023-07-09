using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Runtime.ConstrainedExecution;

public class Tile : MonoBehaviour {
	[SerializeField] private Sprite[] tileSprites;
    [SerializeField] private Color normalColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject visionLight;
    [SerializeField] private GameObject lightingDim;
    [SerializeField] private GameObject lightingDark;
    
    [SerializeField] private bool isWalkable = true;
    [SerializeField] private int LightLevel;

	public bool isWall = false;
    public BaseUnit OccupyingUnit = null;
    private bool Selected = false;
    public bool Walkable => isWalkable && OccupyingUnit == null;

    public void Init(bool isOffset) {
        renderer.color = isOffset ? offsetColor :  normalColor;
        isWalkable = true;
    }

	public void Init(Variant variant) {
        lightingDark.SetActive(true);
		if (variant == Variant.empty) {
			variant = Variant.pathEmpty;
		}
		renderer.sprite = tileSprites[(int) variant];
		if ((int) variant >= 13 && (int) variant <= 18) {
			isWalkable = true;
		} else {
			isWalkable = false;
			isWall = true;
		}
	}
    void Update() {
        if(Selected && OccupyingUnit != null) {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector2 inputDirection = new(x,y);
            inputDirection = inputDirection.normalized;
            if(GameManager.Instance.cardinals.Contains(inputDirection)) {
                OccupyingUnit.Rotate(inputDirection);
            }
        }
    }

    public void ToggleSelected() {
		Selected = !Selected;
		highlight.SetActive(Selected);
    }

    void OnMouseEnter() {
        highlight.SetActive(true);
    }

    void OnMouseExit() {
        if(!Selected) { 
            highlight.SetActive(false);
        }
    }

    void OnMouseDown() {
		if (GameManager.Instance.GameState == GameState.Simulation) {
			return;
		}
        ToggleSelected();
        GameManager.Instance.SelectTile(this);
    }

    public void SetLightLevel(int level) {
        LightLevel = level;
    }

    public void VisionHighlight() {
		if (!isWall) {
	        visionLight.SetActive(true);
		}
    }

    public void VisionUnhighlight() {
        visionLight.SetActive(false);
    }

    public void SetUnit(BaseUnit baseUnit) {
        if(baseUnit.OccupiedTile != null) {
            baseUnit.OccupiedTile.OccupyingUnit = null;
        }
		if(OccupyingUnit != null) {
			OccupyingUnit.OccupiedTile = null;
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
