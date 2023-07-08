using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;
    public Tile SelectedTile = null;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.MakeGrid);
    }

    public void SelectTile(Tile tile) {
        if(GameState == GameState.SelectSquare) { // A tile should be selected
            if(tile == SelectedTile) { // We double click on same tile: unselect
                SelectedTile = null;
                ChangeState(GameState.EmptyState);
            } // Our selected has a guard -> we are moving it
            else if(tile.OccupyingUnit == null && SelectedTile.OccupyingUnit != null && SelectedTile.OccupyingUnit is BaseGuard) {
                MoveGuard(tile, SelectedTile);
                tile.ToggleSelected();
                SelectedTile.ToggleSelected();
                SelectedTile = null;
            } else { // Select a new tile
                SelectedTile.ToggleSelected();
                SelectedTile = tile;
            }
        } else {
            ChangeState(GameState.SelectSquare);
            SelectedTile = tile;
        }
    }

    public void MoveGuard(Tile destination, Tile source) {
        var Guard = source.OccupyingUnit;
        destination.SetUnit(Guard);
        GameState = GameState.SpawnGuards;
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.MakeGrid:
                GridController.Instance.MakeGrid();
                break;
            case GameState.SpawnGuards:
                UnitManager.Instance.SpawnGuards();
                break;
            case GameState.SpawnIntruder:
                UnitManager.Instance.SpawnIntruder();
                break;
            case GameState.TickUp:
                break;
            case GameState.SelectSquare:
                break;
            case GameState.EmptyState:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    MakeGrid = 0,
    SpawnGuards = 1,
    SpawnIntruder = 2,
    TickUp = 3,
    SelectSquare = 4,
    EmptyState = 5
}
