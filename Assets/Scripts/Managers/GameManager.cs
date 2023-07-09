using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;
	public Level level;
    public Tile SelectedTile = null;
    public GaurdProfile GaurdProfile = null;
    public Vector2[] cardinals = {Vector2.down, Vector2.left, Vector2.up, Vector2.right};
	public Sprite[] buttonSprites;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
		level = LevelLoader.GetLevel("Test");
        ChangeState(GameState.MakeGrid);
		ChangeState(GameState.SpawnGuards);
    }

    public void SelectTile(Tile tile)
    {
		if (GameState == GameState.Simulation) {
			return;
		}
        if (GameState == GameState.SelectSquare)
        { // A tile should be selected
            if (tile == SelectedTile)
            { // We double click on same tile: unselect
                SelectedTile = null;
                ChangeState(GameState.EmptyState);
            } // Our selected has a guard -> we are moving it
            else if (tile.OccupyingUnit == null && SelectedTile.OccupyingUnit != null && SelectedTile.OccupyingUnit is BaseGuard && !tile.isWall)
            {
                MoveGuard(tile, SelectedTile);
                tile.ToggleSelected();
                SelectedTile.ToggleSelected();
                SelectedTile = null;
            }
            else
            { // Select a new tile
                SelectedTile.ToggleSelected();
                SelectedTile = tile;
            }
        }
        else if (GameState == GameState.SpawnGuard)
        {
            if (tile.OccupyingUnit == null)
                SpawnGaurd(tile, GaurdProfile.ProfileGaurd);
            tile.ToggleSelected();
            SelectedTile = null;
        }
        else
        {
            ChangeState(GameState.SelectSquare);
            SelectedTile = tile;
        }
    }

    public void SelectGaurdProfile(GaurdProfile profile)
    {
		if (GameState == GameState.Simulation) {
			return;
		}
        GaurdProfile = profile;
        if (SelectedTile != null)
        {
            SelectedTile.ToggleSelected();
            SelectedTile = null;
        }
        ChangeState(GameState.SpawnGuard);
    }

    public void SpawnGaurd(Tile Destination, BaseGuard Guard)
    {
        var newGuard = Instantiate(Guard);
        newGuard.transform.localScale = new Vector2(1, 1);
        Destination.SetUnit(newGuard);
        GaurdProfile.ToggleSelected();
        GaurdProfile = null;
        ChangeState(GameState.EmptyState);
    }

    public void MoveGuard(Tile destination, Tile source)
    {
        var Guard = source.OccupyingUnit;
        destination.SetUnit(Guard);
        GameState = GameState.EmptyState;
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
            case GameState.SpawnGuard:
                break;
			case GameState.Simulation:
				break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

	public void StartGame() {
		foreach (Intruder1 GameObject in FindObjectsOfType<Intruder1>()) {
			GameObject.gameObject.SetActive(false);
			Destroy(GameObject.gameObject);
		}
		ChangeState(GameState.SpawnIntruder);
		ChangeState(GameState.Simulation);
		TickManager.Instance.active = true;
		// Change button sprite to "back"
		GameObject button = GameObject.Find("GoButton");
		button.GetComponent<UnityEngine.UI.Image>().sprite = buttonSprites[1];
		button.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

		// Set button onclick to gameover
		button.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
		button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(GameOver);
		
	}

	public void GameOver() {
		TickManager.Instance.active = false;
		// Change button sprite to "play"
		GameObject button = GameObject.Find("GoButton");
		button.GetComponent<UnityEngine.UI.Image>().sprite = buttonSprites[0];
		button.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

		// Set button onclick to startgame
		button.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
		button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
		
		// Reset guards
		UnitManager.Instance.ResetUnits();

		// Set state back to spawn guards
		GameState = GameState.SpawnGuards;
	}

	public void GuardsAlerted() {
		Debug.Log("Guards alerted! Game over!");
		GameOver();
	}
}

public enum GameState
{
    MakeGrid = 0,
    SpawnGuards = 1,
    SpawnIntruder = 2,
    TickUp = 3,
    SelectSquare = 4,
    EmptyState = 5,
    SpawnGuard = 6,
	Simulation = 7
}
