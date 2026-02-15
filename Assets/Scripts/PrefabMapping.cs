using System.Collections;
using UnityEngine;

[System.Serializable]
public class PrefabMapping
{
    public string key;      // e.g., "WildHog", "IronPlate", "IronOre", "RoadBarrier", or etc..
                            // Also: "exitTrigger" to mark the end of a level
    public GameObject prefab;
}
