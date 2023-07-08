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
            var randomTile = GridController.Instance.GetGuardSpawnTile().GetComponent<Tile>();

            randomTile.SetUnit(newGuard);
        }
    }

    private T GetRandomUnit<T> (Type type) where T : BaseUnit {
        return (T) units.Where(unit => unit.type == type).OrderBy(o => Random.value).First().UnitPrefab;
    }
}
