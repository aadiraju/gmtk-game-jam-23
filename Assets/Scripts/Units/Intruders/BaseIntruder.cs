using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIntruder : BaseUnit {
	public int x, y;
	public int gridWidth;
	public int gridHeight;
	public string path;
	private int pathIndex = 0;
	private bool reachedEndOfPath = false;
	private int tick = 0;
	
	
    void Start() {}

	override public void TickUp() {
		if (reachedEndOfPath) {
			Debug.Log("Intruder reached end of path");
			TickManager.Instance.removeIntruder(this);
			return;
		}

		Direction direction = GetDirection(pathIndex++);
		switch (direction) {
			case Direction.Up:
				y++;
				break;
			case Direction.Down:
				y--;
				break;
			case Direction.Left:
				x--;
				break;
			case Direction.Right:
				x++;
				break;
		}

		if (x < 0 || x >= GridController.Instance.height || y < 0 || y >= GridController.Instance.width) {
			throw new System.ArgumentException("Intruder went out of bounds. Write a proper path mate.");
		}

		// transform.position = new Vector3(ToGridX(x), ToGridY(y), 0f);
		SetPosition(x, y);

		if (pathIndex >= path.Length) {
			reachedEndOfPath = true;
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

	private enum Direction {
		Up,
		Down,
		Left,
		Right
	}
}
