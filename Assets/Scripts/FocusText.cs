using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusText : MonoBehaviour
{
    private string DefaultMessage = "Click a square for more details.";
    // Start is called before the first frame update
    private void SetText(string msg) {
        gameObject.GetComponent<Text>().text = msg;
    }

    public void ClearText() {
        SetText(DefaultMessage);
    }
    public void SetTextFromTile(Tile tile) {
        SetText(GenerateText(tile));
    }

    private string GenerateText(Tile tile) {
        if(tile == null) {
            return DefaultMessage;
        } else if(tile.isWall){
            return "Wall. Units cannot be placed here.";
        } else if(tile.Walkable){
            return "Empty space. Units can be moved here.";
        } else if (tile.OccupyingUnit is BaseGuard) {
            return string.Format("{0}. {1}", tile.OccupyingUnit.Title(), tile.OccupyingUnit.Description());
        } else {
            return "Error.";
        }
    }

    void Start()
    {
        ClearText();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.SelectSquare) {
            SetTextFromTile(GameManager.Instance.SelectedTile);
        } else if(GameManager.Instance.GameState == GameState.EmptyState) {
            ClearText();
        }
    }
}
