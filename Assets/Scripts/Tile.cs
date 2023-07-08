using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Runtime.ConstrainedExecution;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color normalColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;
    
    [SerializeField] private bool isWalkable = true;
    [SerializeField] private int LightLevel;

    public BaseUnit OccupyingUnit = null;
    private bool Selected = false;
    public bool Walkable => isWalkable && OccupyingUnit == null;

    public void Init(bool isOffset) {
        renderer.color = isOffset ? offsetColor :  normalColor;
        isWalkable = true;
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
        Highlight();
    }

    void OnMouseExit() {
        if(!Selected) { 
            highlight.SetActive(false);
        }
    }

    void OnMouseDown() {
        ToggleSelected();
        GameManager.Instance.SelectTile(this);
    }

    public void Highlight() {
        highlight.SetActive(true);
    }

    public void Unhighlight() {
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
}
