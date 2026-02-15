using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Reference to the LevelLoader component that loads the JSON level data
    public LevelLoader levelLoader;

    // Spawn point references
    // xStartPos for the x position, y will come from each spawn event, z is constant
    private float xStartPos = -3f;
    //public Transform spawnPoint;
    private float zPosition = -4f; // matches player's z-position as positioned in editor

    // Mapping from prefab keys to prefab objects; set these in the Inspector
    public PrefabMapping[] prefabMappings;

    // Reference to Player and player components, e.g. so we can stop at a gameover.
    public GameObject playerGO;
    public PlayerController playerControllerScript;
    public MovePlayerLeft runOffScreenTrigger;
    public StopMovePlayerLeft stopRunningOffScreen;

    // Constants for winCondition int in Player Controller used to show final score text
    public int GAMELOST = 1;
    public int GAMEWON = 2;

    void Start()
    {
        // Get a reference to the Player for limiting actions based on gameover
        playerGO = GameObject.Find("Player");
        playerControllerScript = playerGO.GetComponent<PlayerController>();
        runOffScreenTrigger = playerGO.GetComponent<MovePlayerLeft>();
        stopRunningOffScreen = playerGO.GetComponent<StopMovePlayerLeft>();

        // Set back to false in case it was on from the last round.
        stopRunningOffScreen.enabled = false;

        // Get reference to level loader
        levelLoader = GetComponent<LevelLoader>();
    }

    public void StartGame()
    {
        // Start the spawning routine that processes the JSON-defined events
        Debug.Log("Starting the Spawner...");
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // Wait until the level data is loaded (Future Nate: Add a timeout to prevent an infinite loop)
        while (levelLoader.levelDataJson == null)
        {
            Debug.Log("Waiting for level data to load...");
            yield return null;
        }
        Debug.Log("Level data loading...");

        // Iterate through each spawn event defined in the JSON level data
        foreach (SpawnEvent spawnEvent in levelLoader.levelDataJson.spawnEvents)
        {
            Debug.Log("Spawner started!");
            // Wait the delay specified for this spawn event
            yield return new WaitForSeconds(spawnEvent.delay);

            // If the game is over, stop spawning further objects
            if (playerControllerScript.gameOver)
            {
                yield break;
            }
            else
            {
                Debug.Log("Game not over!");
            }

            // Calculate the spawn location: x from spawnPoint, y from the spawn event, constant z
            Vector3 spawnLocation = new Vector3(xStartPos, spawnEvent.spawnY, zPosition);

            // Look up the prefab for this spawn event using the prefabKey
            GameObject prefabToSpawn = GetPrefabByKey(spawnEvent.prefabKey);

            // If game is still active, spawn new object
            if (prefabToSpawn != null && !playerControllerScript.gameOver && prefabToSpawn.name != "exitTrigger")     
            {
                Debug.Log("Spawning in a " + prefabToSpawn + " object now!");
                Instantiate(prefabToSpawn, spawnLocation, prefabToSpawn.transform.rotation);
            }
            else if (prefabToSpawn.name == "exitTrigger")
            {
                Debug.Log("End of level trigger found; exiting level in 7 seconds...");
                // Wait 7 seconds before starting level complete sequence
                // Timing is based on when testing showed that the sequence was started
                // compared to when it needs to finish...
                yield return new WaitForSeconds(7);
                YouWin();
            }
            else
            {
                Debug.LogError("No prefab found for key: " + spawnEvent.prefabKey);
            }
        }
    }

    // Helper method to retrieve a prefab based on a key
    GameObject GetPrefabByKey(string key)
    {
        foreach (PrefabMapping mapping in prefabMappings)
        {
            if (mapping.key == key)
            {
                return mapping.prefab;
            }
        }
        return null;
    }

    private void YouWin()
    {
        Debug.Log("Beginning YouWin() now.");

        // Disable player control (indirectly)
        playerControllerScript.gameCompleted = true;
        Debug.Log("Disabled player control.");

        // Run off screen (activates MovePlayerLeft script)
        runOffScreenTrigger.enabled = true;
        Debug.Log("Asked player to run off screen.");

        // Fade out music over 5 seconds
        playerControllerScript.FadeOutMusic(5f);
        Debug.Log("Asked music to fade out.");

        // Display "You win!" text and final score,
        // then wait for player input to restart
        playerControllerScript.FinalScore(GAMEWON);
        Debug.Log("Asked final score to display.");

        // Stop the running once player is off screen
        stopRunningOffScreen.enabled = true;
        //Debug.Log("Asked Unity to destroy player object.");
    }
}
