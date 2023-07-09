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
	private List<BaseGuard> guards;
	private BaseIntruder intruder;

    void Awake() {
        Instance = this;

        units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
		guards = new List<BaseGuard>();
    }

    public void SpawnGuards() {
        for (int i = 0; i < guardCount; i++) {
            var randomPrefab = GetRandomUnit<BaseGuard>(Type.Guard);
            var newGuard = Instantiate(randomPrefab);
            var randomTile = GridController.Instance.GetRandomFreeTile().GetComponent<Tile>();
            newGuard.ToggleActive();
            randomTile.SetUnit(newGuard);
			TickManager.Instance.addGuard(newGuard);
			guards.Add(newGuard);
        }
    }

	public void ResetUnits() {
		foreach (var guard in guards) {
			guard.Reset();
		}

		TickManager.Instance.removeIntruder();
		Destroy(intruder.gameObject);
		intruder = null;
	}

	public void SpawnIntruder() {
		Level level = GameManager.Instance.level;
		var randomPrefab = GetRandomUnit<BaseIntruder>(Type.Intruder);
		var newIntruder = Instantiate(randomPrefab);
		var randomTile = GridController.Instance.GetTileAtPosition(new Vector2(level.intruderPaths[0].startX, level.intruderPaths[0].startY));
		newIntruder.path = level.intruderPaths[0].path;
		newIntruder.gameObject.SetActive(true);

		randomTile.SetUnit(newIntruder);
		TickManager.Instance.addIntruder(newIntruder);
		intruder = newIntruder;
	}

	public void addGuard(BaseGuard guard) {
		guards.Add(guard);
	}

    private T GetRandomUnit<T> (Type type) where T : BaseUnit {
        return (T) units.Where(unit => unit.type == type).OrderBy(o => Random.value).First().UnitPrefab;
    }
}
