using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
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

    void OnMouseEnter() {
        Highlight();
    }

    void OnMouseExit() {
        highlight.SetActive(false);
    }

    public void Highlight() {
        highlight.SetActive(true);
    }

    public void SetUnit(BaseUnit baseUnit) {
        if(baseUnit.OccupiedTile != null) {
            baseUnit.OccupiedTile.OccupyingUnit = null;
        }
        baseUnit.transform.position = transform.position;
        OccupyingUnit = baseUnit;
        baseUnit.OccupiedTile = this;
    }
}
