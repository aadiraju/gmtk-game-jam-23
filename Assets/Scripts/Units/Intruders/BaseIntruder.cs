using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class BaseIntruder : BaseUnit {
	public int x, y;
	public int gridWidth;
	public int gridHeight;
	public string path;
	private int pathIndex = 0;
	private bool reachedEndOfPath = false;
	Animator animController;
	
	
    void Start() {}

	void Awake() {
		animController = GetComponent<Animator>();
	}

	override public void TickUp() {
		if (reachedEndOfPath) {
			TickManager.Instance.removeIntruder();
			return;
		}

		Direction direction = GetDirection(pathIndex++);
		switch (direction) {
			case Direction.Up:
				y++;
				Rotate(Vector2.up);
				break;
			case Direction.Down:
				y--;
				Rotate(Vector2.down);
				break;
			case Direction.Left:
				x--;
				Rotate(Vector2.left);
				break;
			case Direction.Right:
				x++;
				Rotate(Vector2.right);
				break;
		}

		if (x < 0 || x >= GridController.Instance.width || y < 0 || y >= GridController.Instance.height) {
			throw new System.ArgumentException("Intruder went out of bounds. Write a proper path mate.");
		}
		bool fixOccupiedTile = false;
		Tile tileFix = null;
		BaseGuard guardFix = null;
		if (!GridController.Instance.GetTileAtPosition(new Vector2(x, y)).Walkable) {
			// Check if tile is a wall or guard
			if (GridController.Instance.GetTileAtPosition(new Vector2(x, y)).isWall) {
				throw new System.ArgumentException("Intruder tried to walk into a wall. Write a proper path mate.");
			} else {
				BaseUnit unit = GridController.Instance.GetTileAtPosition(new Vector2(x, y)).OccupyingUnit;
				if (unit is GoldShroomController) {
					GameManager.Instance.IntruderReachedGoal();
					return;
				}
				if (unit is BaseGuard) {
					BaseGuard guard = (BaseGuard)unit;
					guard.EraseVisionCone();
					guard.gameObject.SetActive(false);
					fixOccupiedTile = true;
					guardFix = guard;
					tileFix = guard.OccupiedTile;
				}
			}
		}

		SetPosition(x, y);

		if (fixOccupiedTile) {
			guardFix.OccupiedTile = tileFix;
		}

		if (pathIndex >= path.Length) {
			reachedEndOfPath = true;
			GameManager.Instance.IntruderReachedGoal();
		}
	}

	public void SetPosition(int x, int y) {
		GridController.Instance.GetTileAtPosition(new Vector2(x, y)).SetUnit(this);
	}

	private float ToGridX(int x) {
		return x - (gridWidth - 1) / 2 - 0.5f;
	}

	private float ToGridY(int y) {
		return y - (gridHeight - 1) / 2 + 0.5f;
	}

	private Direction GetDirection(int n) {
		return path[n] switch {
			'U' => Direction.Up,
			'D' => Direction.Down,
			'L' => Direction.Left,
			'R' => Direction.Right,
			_ => throw new System.ArgumentException("Invalid direction in path: '" + path[n] + "'")
		};
	}

	 public override void Rotate(Vector2 direction) {
		lookDirection = direction;
        animController.SetFloat("Move X", direction.x);
        animController.SetFloat("Move Y", direction.y);
    }

	public void TriggerCaughtAnim() {
		animController.SetTrigger("Caught");
	}

	private enum Direction {
		Up,
		Down,
		Left,
		Right
	}
}
