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
    public Vector2[] cardinals = { Vector2.down, Vector2.left, Vector2.up, Vector2.right };
    public Sprite[] buttonSprites;
    public Vector2 GoldenShroomLocation = new(0, 0);
    public SoundHandler sh;
    public int simulationNumber = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        level = LevelLoader.GetLevel("Test");
        ChangeState(GameState.MakeGrid);
        ChangeState(GameState.SpawnGuards);
        sh.PlayPlanningMusic();
    }

    public void SelectTile(Tile tile)
    {
        if (GameState == GameState.Simulation)
        {
            return;
        }
        sh.PlayClick();
        if (GameState == GameState.SelectSquare)
        { // A tile should be selected
            if (tile == SelectedTile)
            { // We double click on same tile: unselect
                SelectedTile = null;
                ChangeState(GameState.EmptyState);
            } // Our selected has a guard -> we are moving it
            else if (tile.OccupyingUnit == null && SelectedTile.OccupyingUnit != null && SelectedTile.OccupyingUnit is BaseGuard && !tile.isWall && SelectedTile.OccupyingUnit is not GoldShroomController)
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
            if (tile.OccupyingUnit == null && !tile.isWall)
                SpawnGaurd(tile, GaurdProfile.ProfileGaurd);
            else
            {
                GaurdProfile.ToggleSelected();
                GaurdProfile = null;
            }
            tile.ToggleSelected();
            SelectedTile = null;
            ChangeState(GameState.EmptyState);
        }
        else
        {
            ChangeState(GameState.SelectSquare);
            SelectedTile = tile;
        }
    }

    public void SelectGaurdProfile(GaurdProfile profile)
    {
        if (GameState == GameState.Simulation)
        {
            return;
        }
        GaurdProfile = profile;
        ChangeState(profile == null ? GameState.EmptyState : GameState.SpawnGuard);
        if (SelectedTile != null)
        {
            SelectedTile.ToggleSelected();
            SelectedTile = null;
        }
    }

    public void SpawnGaurd(Tile Destination, BaseGuard Guard)
    {
        var newGuard = Instantiate(Guard);
        newGuard.transform.localScale = new Vector2(1, 1);
        newGuard.ToggleActive();
        TickManager.Instance.addGuard(newGuard);
        UnitManager.Instance.addGuard(newGuard);
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

    public void StartGame()
    {
        simulationNumber = 0;
        foreach (Intruder1 GameObject in FindObjectsOfType<Intruder1>())
        {
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

        sh.StopPlanningMusic();
        sh.PlaySimulationMusic();
    }

    public void GameOver()
    {
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
        GameState = GameState.EmptyState;

        // Stop simulation music
        sh.StopSimulationMusic();
        sh.PlayPlanningMusic();
    }

    public void DeleteGuard()
    {
        if (SelectedTile != null && SelectedTile?.OccupyingUnit != null && SelectedTile?.OccupyingUnit is BaseGuard && !(SelectedTile?.OccupyingUnit is GoldShroomController))
        {
            TickManager.Instance.removeGuard((BaseGuard)SelectedTile.OccupyingUnit);
            UnitManager.Instance.removeGuard((BaseGuard)SelectedTile.OccupyingUnit);
            ((BaseGuard)SelectedTile.OccupyingUnit).EraseVisionCone();
            Destroy(SelectedTile.OccupyingUnit.gameObject);
            SelectedTile.OccupyingUnit = null;
            SelectedTile.ToggleSelected();
            SelectedTile = null;
            ChangeState(GameState.EmptyState);
        }
    }


    public void IntruderReachedGoal()
    {
        Debug.Log("Intruder reached goal! Game over!");
        sh.PlayLoss();
        GameOver();
    }

    public void GuardsAlerted()
    {
        Debug.Log("Guards alerted!");
        StartCoroutine(TriggerIntruderDeath());
    }

    public void NextSimulation()
    {
        simulationNumber++;
        if (simulationNumber >= level.intruderPaths.Count)
        {
            Victory();
        }
        else
        {
            TickManager.Instance.active = false;
            UnitManager.Instance.ResetUnits();
            ChangeState(GameState.SpawnIntruder);
            TickManager.Instance.active = true;
        }
    }

    private void Victory()
    {
        Debug.Log("Victory!");
        sh.PlayVictory();
        GameOver();
    }

    IEnumerator TriggerIntruderDeath()
    {
        TickManager.Instance.intruder.TriggerCaughtAnim();
        TickManager.Instance.active = false;
        yield return new WaitForSeconds(2f); //wait for 2s to finish animation, then call game over
        NextSimulation();
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

public enum EndState
{
    Victory = 0,
    NextSimulation = 1,
    Loss = 2
}
