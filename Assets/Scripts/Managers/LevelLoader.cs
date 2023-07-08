using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class LevelLoader {
	private static Color pathDirt = new Color(0.725f, 0.478f, 0.341f, 1.000f);  // Paint "brown"
	private static Color pathGrass = new Color(0.710f, 0.902f, 0.114f, 1.000f);  // Paint "lime"
	private static Color pathGrassThick = new Color(0.133f, 0.694f, 0.298f, 1.000f);  // Paint "green"
	private static Color pathStone = new Color(0.765f, 0.765f, 0.765f, 1.000f);  // Paint "light grey"
	private static Color pathMushroom = new Color(1.000f, 0.949f, 0.000f, 1.000f);  // Paint "yellow"
	private static Color largeStone = new Color(0.498f, 0.498f, 0.498f, 1.000f);  // Paint "grey"
	private static Color stump = new Color(0.533f, 0.000f, 0.082f, 1.000f);  // Paint "dark red"
	public static Level GetLevel(string levelName) {
		// Read the JSON file
        string jsonContent = File.ReadAllText("Assets/Resources/Levels/" + levelName + "/level.json");

        // Deserialize the JSON into Level object
        Level level = JsonConvert.DeserializeObject<Level>(jsonContent);
		level.name = levelName;

		Texture2D texture = Resources.Load<Texture2D>("Levels/" + levelName + "/level");
		level.mapWidth = texture.width;
		level.mapHeight = texture.height;
		level.map = new Tile.Variant[texture.width, texture.height];
		for (int x = 0; x < texture.width; x++) {
			for (int y = 0; y < texture.height; y++) {
				Color pixelColor = texture.GetPixel(x, y);
				if (ColorEqual(pixelColor, Color.white)) {
					level.map[x, y] = Tile.Variant.empty;
				} else if (ColorEqual(pixelColor, Color.black)) {
					// Determine which wall variant to use
					
					// Get the surrounding pixels
					Color[] surroundingPixels = new Color[8];
					surroundingPixels[0] = texture.GetPixel(x - 1, y + 1);
					surroundingPixels[1] = texture.GetPixel(x, y + 1);
					surroundingPixels[2] = texture.GetPixel(x + 1, y + 1);
					surroundingPixels[3] = texture.GetPixel(x - 1, y);
					surroundingPixels[4] = texture.GetPixel(x + 1, y);
					surroundingPixels[5] = texture.GetPixel(x - 1, y - 1);
					surroundingPixels[6] = texture.GetPixel(x, y - 1);
					surroundingPixels[7] = texture.GetPixel(x + 1, y - 1);

					// Check if the surrounding pixels are black or white
					bool[] surroundingPixelsWhite = new bool[8];
					for (int i = 0; i < 8; i++) {
						surroundingPixelsWhite[i] = surroundingPixels[i] != Color.black;
					}

					// Decide on the variant
					if (surroundingPixelsWhite[1] && !surroundingPixelsWhite[3] && !surroundingPixelsWhite[4]) {
						level.map[x,y] = Tile.Variant.wallEdge0;
					} else if (surroundingPixelsWhite[4] && !surroundingPixelsWhite[1] && !surroundingPixelsWhite[6]) {
						level.map[x,y] = Tile.Variant.wallEdge1;
					} else if (surroundingPixelsWhite[6] && !surroundingPixelsWhite[3] && !surroundingPixelsWhite[4]) {
						level.map[x,y] = Tile.Variant.wallEdge2;
					} else if (surroundingPixelsWhite[3] && !surroundingPixelsWhite[1] && !surroundingPixelsWhite[6]) {
						level.map[x,y] = Tile.Variant.wallEdge3;
					} else if (surroundingPixelsWhite[0] && surroundingPixelsWhite[1] && surroundingPixelsWhite[3]) {
						level.map[x,y] = Tile.Variant.wallCorner0;
					} else if (surroundingPixelsWhite[1] && surroundingPixelsWhite[4] && !surroundingPixelsWhite[3]) {
						level.map[x,y] = Tile.Variant.wallCorner1;
					} else if (surroundingPixelsWhite[4] && surroundingPixelsWhite[6] && surroundingPixelsWhite[7]) {
						level.map[x,y] = Tile.Variant.wallCorner2;
					} else if (surroundingPixelsWhite[3] && surroundingPixelsWhite[5] && surroundingPixelsWhite[6]) {
						level.map[x,y] = Tile.Variant.wallCorner3;
					} else if (surroundingPixelsWhite[7]) {
						level.map[x,y] = Tile.Variant.wallCorner4;
					} else if (surroundingPixelsWhite[5]) {
						level.map[x,y] = Tile.Variant.wallCorner5;
					} else if (surroundingPixelsWhite[0]) {
						level.map[x,y] = Tile.Variant.wallCorner6;
					} else if (surroundingPixelsWhite[2]) {
						level.map[x,y] = Tile.Variant.wallCorner7;
					} else {
						level.map[x,y] = Tile.Variant.wallCenter;
					}
				} else if (ColorEqual(pixelColor, pathDirt)) {
					level.map[x, y] = Tile.Variant.pathDirt;
				} else if (ColorEqual(pixelColor, pathGrass)) {
					level.map[x, y] = Tile.Variant.pathGrass;
				} else if (ColorEqual(pixelColor, pathGrassThick)) {
					level.map[x, y] = Tile.Variant.pathThickGrass;
				} else if (ColorEqual(pixelColor, pathStone)) {
					level.map[x, y] = Tile.Variant.pathStoney;
				} else if (ColorEqual(pixelColor, pathMushroom)) {
					level.map[x, y] = Tile.Variant.pathMushrooms;
				} else if (ColorEqual(pixelColor, largeStone)) {
					level.map[x, y] = Tile.Variant.largeStone;
				} else if (ColorEqual(pixelColor, stump)) {
					level.map[x, y] = Tile.Variant.stump;
				} else {
					level.map[x, y] = Tile.Variant.empty;
					Debug.Log("Unknown color: " + pixelColor);
				}
			}
		}

		return level;
	}

	private static bool ColorEqual(Color a, Color b) {
		return a.ToString() == b.ToString();
	}
}

public class IntruderPath {
	public int startX { get; set; }
	public int startY { get; set; }
	public string path { get; set; }
}

public class Level {
	public string name { get; set; }
	public int mapWidth { get; set; }
	public int mapHeight { get; set; }
	public Tile.Variant[,] map { get; set; }
	public IList<IntruderPath> intruderPaths { get; set; }
}
