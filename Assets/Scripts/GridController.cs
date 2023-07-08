using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class GridController : MonoBehaviour
{
    public static GridController Instance;
    [SerializeField] private int width, height;
    [SerializeField] private float startX, startY;
    [SerializeField] private GameObject TilePrefab;

    private Dictionary<Vector2, GameObject> tiles;
    // Start is called before the first frame update
    void Awake() {
        Instance = this;
    }

    // Update is called once per frame
   public void MakeGrid() {
        tiles = new Dictionary<Vector2, GameObject>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var newTile = Instantiate(TilePrefab, new Vector2(startX + x,startY + y), Quaternion.identity);
                newTile.name = $"Tile {x} {y}";
                newTile.GetComponent<Tile>().Init((x + y) % 2 == 1);

                tiles[new Vector2(x,y)] = newTile;
            }
        }

        GameManager.Instance.ChangeState(GameState.SpawnGuards);
    }

    public GameObject GetTileAtPosition(Vector2 pos) {
        if(tiles.TryGetValue(pos, out var tile)) {
            return tile;
        }

        return null;
    }

    public GameObject GetGuardSpawnTile() {
        return tiles.Where(tile => tile.Value.GetComponent<Tile>().Walkable).OrderBy(o => Random.value).First().Value;
    }
}
