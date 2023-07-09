using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class GridController : MonoBehaviour
{
    public static GridController Instance;
    [SerializeField] public int width, height;
    [SerializeField] private float startX, startY;
    [SerializeField] private GameObject TilePrefab;

    private Dictionary<Vector2, GameObject> tiles;
    // Start is called before the first frame update
    void Awake() {
        Instance = this;
    }

   public void MakeGrid() {
		Level level = GameManager.Instance.level;
		if (level.mapWidth != width || level.mapHeight != height) {
			throw new System.ArgumentException("Level map size does not match grid size.");
		}
		Tile.Variant[,] map = level.map;

        tiles = new Dictionary<Vector2, GameObject>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var newTile = Instantiate(TilePrefab, new Vector2(startX + x,startY + y), Quaternion.identity);
                newTile.name = $"Tile {x} {y}";
                newTile.GetComponent<Tile>().Init(map[x, y]);
                tiles[new Vector2(x,y)] = newTile;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos) {
        if(tiles.TryGetValue(pos, out var tile)) {
            return tile.GetComponent<Tile>();
        }

        return null;
    }

    public GameObject GetRandomFreeTile() {
        return tiles.Where(tile => tile.Value.GetComponent<Tile>().Walkable).OrderBy(o => Random.value).First().Value;
    }
}
