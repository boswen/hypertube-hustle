using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelLoader : MonoBehaviour
{
    public string levelFileName = "level1.json"; // JSON file name in StreamingAssets
    public LevelDataJson levelDataJson;

    void Awake()
    {
        LoadLevelData();
        Debug.Log("Presumably.. exiting the LevelLoader class now.");
    }

    void LoadLevelData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, levelFileName);
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            levelDataJson = JsonUtility.FromJson<LevelDataJson>(jsonString);
            Debug.Log("Loaded " + levelDataJson.spawnEvents.Length + " spawn events.");
        }
        else
        {
            Debug.LogError("Level data file not found: " + filePath);
        }
    }
}
