using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class IntruderTest : MonoBehaviour {
	public string levelJSON;
	private Level level;
	private Intruder intruder;

	void Start() {
		LoadLevelJSON();
		
	}

	void Update() {

	}

	public void LoadLevelJSON() {
		// Read the JSON file
        string jsonContent = File.ReadAllText(levelJSON);

        // Deserialize the JSON into Level object
        Level data = JsonConvert.DeserializeObject<Level>(jsonContent);
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
