using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LevelLoader : MonoBehaviour
{
    public string levelFileName = "level1.json"; // must live in Assets/StreamingAssets/
    public LevelDataJson levelDataJson;

    private void Awake()
    {
        // Kick off load. In WebGL this must be async.
        StartCoroutine(LoadLevelDataCoroutine());
    }

    private IEnumerator LoadLevelDataCoroutine()
    {
        string pathOrUrl = Path.Combine(Application.streamingAssetsPath, levelFileName);

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL: StreamingAssetsPath is a URL, so use UnityWebRequest
        Debug.Log($"[LevelLoader] WebGL loading: {pathOrUrl}");
        using (UnityWebRequest req = UnityWebRequest.Get(pathOrUrl))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[LevelLoader] FAIL {pathOrUrl} :: {req.error} :: {req.responseCode}");
                yield break;
            }

            ParseJson(req.downloadHandler.text);
        }
#else
        // Editor/Standalone: normal file IO works
        if (!File.Exists(pathOrUrl))
        {
            Debug.LogError("Level data file not found: " + pathOrUrl);
            yield break;
        }

        string jsonString = File.ReadAllText(pathOrUrl);
        ParseJson(jsonString);
        yield break;
#endif
    }

    private void ParseJson(string jsonString)
    {
        levelDataJson = JsonUtility.FromJson<LevelDataJson>(jsonString);

        if (levelDataJson == null || levelDataJson.spawnEvents == null)
        {
            Debug.LogError("Level JSON parsed but spawnEvents was null. Check JSON format vs LevelDataJson.");
            return;
        }

        Debug.Log("Loaded " + levelDataJson.spawnEvents.Length + " spawn events.");
    }
}
