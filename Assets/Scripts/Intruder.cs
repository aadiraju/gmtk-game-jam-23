using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intruder : MonoBehaviour, Tickable {
	public int x, y;
	public int gridWidth;
	public int gridHeight;
	public string path;
	private int pathIndex = 0;
	private bool reachedEndOfPath = false;
	private int tick = 0;
	
	
    void Start() {
        transform.position = new Vector3(ToGridX(x), ToGridY(y), 0f);
    }

    void Update() {
        
    }

	void FixedUpdate() {
		if (++tick == 20) {
			tick = 0;
			if (!reachedEndOfPath) {
				TickUp();
			}
		}
	}

	public void TickUp() {
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

		if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight) {
			throw new System.ArgumentException("Intruder went out of bounds. Write a proper path mate.");
		}

		transform.position = new Vector3(ToGridX(x), ToGridY(y), 0f);

		if (pathIndex >= path.Length) {
			reachedEndOfPath = true;
		}
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
