using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SpawnEvent
{
    // No Start() or Update() methods. Instead, this class represents
    // the data for a single spawn event. It is used by the
    // "LevelDataJson" class to define what data will be read in from
    // a JSON file, which is in turn used by the LevelLoader class,
    // which actually reads in the JSON. And that class, in turn,
    // is itself used by the SpawnManager class!

    // Time delay since the previous spawn
    public float delay;

    /* A string representing a prefab to spawn (pickup, enemy, obstacle, platform, etc.)
    KEY:
	- WildHog: Spawns a Wild Hog enemy.
	- IronPlate: Spawns an Iron Plate collectible.
	- IronOre: Spawns an Iron Ore collectible.
	- RoadBarrier: Spawns a Road Barrier obstacle.*/
    public string prefabKey;

    // Vertical (Y) position for the spawn
    public float spawnY;
}
