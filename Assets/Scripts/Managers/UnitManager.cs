using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] private int guardCount = 10;

    private List<ScriptableUnit> units;

    void Awake() {
        Instance = this;

        units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnGuards() {
        for (int i = 0; i < guardCount; i++) {
            var randomPrefab = GetRandomUnit<BaseGuard>(Type.Guard);
            var newGuard = Instantiate(randomPrefab);
            var randomTile = GridController.Instance.GetRandomFreeTile().GetComponent<Tile>();

            randomTile.SetUnit(newGuard);
			TickManager.Instance.addGuard(newGuard);
        }
    }

	public void SpawnIntruder() {
		LevelLoader.Level level = LevelLoader.GetLevel("Assets/Levels/Test/Level.json");
		var randomPrefab = GetRandomUnit<BaseIntruder>(Type.Intruder);
		var newIntruder = Instantiate(randomPrefab);
		var randomTile = GridController.Instance.GetTileAtPosition(new Vector2(level.intruderPaths[0].startX, level.intruderPaths[0].startY));
		newIntruder.path = level.intruderPaths[0].path;

		randomTile.SetUnit(newIntruder);
		TickManager.Instance.addIntruder(newIntruder);
	}

    private T GetRandomUnit<T> (Type type) where T : BaseUnit {
        return (T) units.Where(unit => unit.type == type).OrderBy(o => Random.value).First().UnitPrefab;
    }
}
