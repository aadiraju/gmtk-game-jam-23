using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class LevelLoader {
	public static Level GetLevel(string levelJSON) {
		// Read the JSON file
        string jsonContent = File.ReadAllText(levelJSON);

        // Deserialize the JSON into Level object
        Level data = JsonConvert.DeserializeObject<Level>(jsonContent);

		return data;
	}

	public class IntruderPath {
		public int startX { get; set; }
		public int startY { get; set; }
		public string path { get; set; }
	}

	public class Level {
		public IList<IntruderPath> intruderPaths { get; set; }
	}
}
