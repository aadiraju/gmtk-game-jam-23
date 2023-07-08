using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private float startX, startY;
    [SerializeField] private GameObject TilePrefab;

    private Dictionary<Vector2, GameObject> tiles;
    // Start is called before the first frame update
    void Start()
    {
        MakeGrid();
    }

    // Update is called once per frame
    void MakeGrid() {
        tiles = new Dictionary<Vector2, GameObject>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var newTile = Instantiate(TilePrefab, new Vector2(startX + x,startY + y), Quaternion.identity);
                newTile.name = $"Tile {x} {y}";
                newTile.GetComponent<Tile>().Init((x + y) % 2 == 1);

                tiles[new Vector2(x,y)] = newTile;
            }
        }
    }

    public GameObject GetTileAtPosition(Vector2 pos) {
        if(tiles.TryGetValue(pos, out var tile)) {
            return tile;
        }

        return null;
    }
}
