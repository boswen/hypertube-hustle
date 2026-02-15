using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.InputSystem.EnhancedTouch; // Required for touch input
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Vars
    [Header("Physics Settings")]
    [Tooltip("Adjust in-game physics controls here.")]
    private Rigidbody playerRb;
    // no playerPos Vector3 here -- Player pos doesn't need to be tracked directly...for now
    private Animator playerAnim;
    private GameObject playerGO;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround;
    public float maxHeight; // will eventually be needed when player has a jetpack/hoverpack/etc

    [Header("Audio Settings")]
    //public AudioClip footsteps1Sound;   // These sounds are now played thru the
    //public AudioClip footsteps2Sound;   // PlayerAudioController component, attached
    //public AudioClip jumpSound;         // to the Player GameObject... However,
    public AudioClip itemPickupSound;     // ... these last two are not since they
    public AudioClip crashSound;          // need to be able to play independently.
    public AudioSource pickupNCrashAudio; // Assigned in the inspector
    //public AudioSource footstepJumpAudio; // Assigned in the inspector

    private PlayerAudioController playerAudioController;  // Assigned programmatically below

    [Header("Particle Effect Settings")]
    public ParticleSystem dirtParticle;
    [Tooltip("Attach dirt (from player running) particle effects here!")]
    public ParticleSystem itemPickupParticle;
    [Tooltip("Attach item pickup particle effects here!")]
    public ParticleSystem crashParticle;
    [Tooltip("Attach crash particle effects here!")]

    [Header("Gameover State Settings")]
    public bool gameOver;
    [Tooltip("Has the current round of the game ended by failure?")]
    public bool gameCompleted;
    [Tooltip("Has the current round of the game ended by winning?")]
    private bool crashPlayed;
    [Tooltip("Have the 'game over' state commands executed yet?")]
    public GameObject gameOverText1;
    [Tooltip("Attach game object for line 1 of the game over text here!")]
    public GameObject gameOverText2;
    [Tooltip("Attach game object for line 2 of the game over text here!")]
    public GameObject finalScoreTextGO;
    [Tooltip("Attach game object for the final score at game over text here!")]
    public GameObject mainScoreTextGO;
    [Tooltip("Attach game object for the main score text here!")]
    private TextMeshProUGUI scoreText; // assigned to this at game start if mainScoreTextGO is assigned
    private TextMeshProUGUI firstLineOfText; // assigned to this at game start if gameOverText1 is assigned
    private ScoreManager scoreManager;
    public int finalScore;
    [Tooltip("Final score value; calculated automatically")]
    public float fadeMusicDuration = 5f;
    [Tooltip("Duration of music fade out.")]
    public float fadeObbysDelay = 2f;
    [Tooltip("Delay before obstacles begin to fade out.")]
    public float fadeObbysDuration = 2f;
    [Tooltip("Duration of obstacle fade outs.")]

    public bool isThisThingOn;
    private bool jumpTriggered = false;

    // Input stuff
    private PlayerInput playerInput;
    private InputAction jumpAction;

    // Constants for winCondition int used to show final score text
    public int GAMELOST = 1;
    public int GAMEWON  = 2;

    // -- Primary Methods --
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            // Whoa! I didn't even know this is a thang!
            playerInput = gameObject.AddComponent<PlayerInput>();
        }
        // Get jump action from the input asset
        jumpAction = playerInput.actions["Jump"];
    }

    // Start is called before the first frame update
    void Start()
    {
        //#if UNITY_EDITOR
        //    // Enable touch simulation in Editor
        //    InputSystem.EnableDevice(UnityEngine.InputSystem.Touchscreen.current);
        //    Debug.Log("Touch simulation enabled");
        //#endif

        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        // Connected prepared objects
        playerAudioController = GetComponent<PlayerAudioController>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        scoreText = finalScoreTextGO.GetComponent<TextMeshProUGUI>();
        firstLineOfText = gameOverText1.GetComponent<TextMeshProUGUI>();

        // Reset game state here so that successives rounds run as intended
        ResetPlayerState();
    }

    public void ResetPlayerState()
    {
        // Re-normalize gravity to avoid a cumulative gravity problem!!
        // ... You're welcome. (~Nate, after several hours of debugging!)
        Physics.gravity = Physics.gravity.normalized * 9.81f;
        Physics.gravity *= gravityModifier;

        // Reset Rigidbody physics
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        playerRb.mass = 60f;

        // Temporarily disable Rigidbody to clear forces
        playerRb.isKinematic = true;
        playerRb.isKinematic = false;

        // Reset position and rotation
        transform.position = new Vector3(36f, 0.25f, -4.0f); // Default position (adjust as needed)
        transform.rotation = Quaternion.identity; // Reset to "default" rotation...
        transform.Rotate(0, -90, 0); // ...then reset to the *actual* default rotation!

        // Reset player animator to its default state
        if (playerAnim != null)
        {
            playerAnim.Play("Run_Static");
        }

        // Reset gameplay variables
        //jumpForce = 700f;
        //gravityModifier = 2f;
        //Physics.gravity.Normalize();
        //isOnGround = true;
        gameOver = false;
        gameCompleted = false; // set to True in SpawnManager at end of level event
        crashPlayed = false;
        isThisThingOn = false; // set to true by ToStartPressAnyKey
    }

    // Update is called once per frame
    void Update()
    {
        if (clearedForLiftOff())
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetBool("Grounded", false);
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();

            // Kill the events firer to disable the footsteps sounds
            playerAnim.fireEvents = false;

            // Play landing sound thru secondary audio source
            playerAudioController.Jump();
        }

        // Reset the jump flag after checking
        jumpTriggered = false;
    }

    private bool clearedForLiftOff()
    {
        /* Check if: (player pressed the spacebar, or if player tapped the screen,
         * via jumpTriggered), and if player is on the ground, and if the game is
         * not over (lost), and if the game is not complete (won) and if the game
         * has actually started! */
        //Debug.Log("jumpTriggered = " + jumpTriggered);
        return jumpTriggered && isOnGround && !gameOver && !gameCompleted && isThisThingOn;
    }

    private void OnEnable()
    {
        // Subscribe to the jump action
        jumpAction.performed += OnJumpInput;
    }

    private void OnDisable()
    {
        // Unsubscribe to the jump action
        jumpAction.performed -= OnJumpInput;
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        // Set flag that jump was triggered
        jumpTriggered = true;

        // Log the control that triggered this action -- fodder for debugging
        Debug.Log($"Jump triggered by: {context.control.displayName} from device: {context.control.device.name}");
    }


    private void OnCollisionEnter(Collision collision)
    {
        // If still on ground, play dirt particle effects and footsteps sound
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            isOnGround = true;
            playerAnim.StopPlayback();
            playerAnim.Play("Run_Static");
            playerAnim.SetBool("Grounded", true);
            dirtParticle.Play();

            // Sounds now played thru the "PlayerAudioController" component...*
            //playerAudio.PlayOneShot(footsteps1Sound, 0.3f);

            // Play landing sound thru secondary audio source
            playerAudioController.Land();

            // Re-enable the stupid footsteps...
            playerAnim.fireEvents = true; // allow running footstep sounds to come in from events again
        }

        else if (collision.gameObject.CompareTag("Obstacle") && !crashPlayed)
        {
            TriggerGameOver();
        }

        else if (collision.gameObject.CompareTag("Enemy") && !crashPlayed)
        {
            TriggerGameOver();
        }
    }

    // If Player collides with something, handle it
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ItemPickup"))
        {
            // any bools to set to true?
            // any powerup indicators to set active?
            itemPickupParticle.Play();
            pickupNCrashAudio.PlayOneShot(itemPickupSound, 1.0f); // ... *except this one
            scoreManager.AddPickupBonus(); // add points!
            Destroy(other.gameObject);

            // check child object's tag (to find "ore" or "plate")
            // add the thing picked up to some tabulator, e.g. ironOres++ or ironPlates++

            // StartCoroutine(PowerupCooldown());   // If the powerup has a timelimit, shut er down when it's done
        }
    }

    private void TriggerGameOver()
    {
        gameOver = true;

        // Fade out music
        FadeOutMusic(5f); // Fade over 5 seconds

        // Fade out objects on screen
        FadeOutObjects();

        // Play death animation, crash particle, and crash sound; stop dirt particles
        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 2);
        playerAnim.fireEvents = false; // stop running footstep sounds coming in from events
        crashParticle.Play();
        pickupNCrashAudio.PlayOneShot(crashSound, 1.0f); // ...* and this one.
        dirtParticle.Stop();

        crashPlayed = true; // Prevents it from being played more than once.
        // ... There is a chance that the player can hit a barrel just right and
        // 'permanently' collide with it, causing explosion fx/sfx to repeat!
        // This bool check prevents that!

        // Show Game Over text and final score
        FinalScore(GAMELOST);
    }

    // -- Support Methods --
    public void FadeOutMusic(float fadeDuration)
    {
        // Find the Main Camera's AudioSource component
        AudioSource audioSource = Camera.main.GetComponentInChildren<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on a child object of the Main Camera.");
            return;
        }

        // Start the fade-out coroutine
        StartCoroutine(FadeOut(audioSource, fadeDuration));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        // Gradually reduce volume to 0
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null; // Wait for the next frame
        }

        // Ensure the volume is fully 0 and stop the music
        audioSource.volume = 0f;
        audioSource.Stop();
    }

    public void FadeOutObjects()
    {
        // Find all objects still on screen after game over with the tags "Enemy", "ItemPickup", or "Obstacle"
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("ItemPickup"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Obstacle"));
        // ...and start the fade-out and destroy coroutine for them
        foreach (GameObject obj in gameObjects)
        {
            StartCoroutine(FadeOutAndDestroy(obj, 2f, 2f));
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj, float delay, float fadeDuration)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        MeshRenderer childRenderer = obj.GetComponentInChildren<MeshRenderer>();
        if (childRenderer == null)
        {
            Debug.LogError("No MeshRenderer found in children of " + obj.name);
            Destroy(obj); // If no renderer, just destroy the object
            yield break;
        }

        // Ensure the material instance is unique to avoid affecting other objects
        Material material = childRenderer.material = new Material(childRenderer.material);

        // Set material to Transparent mode
        material.SetFloat("_Mode", 2); // 2 = Transparent
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000; // Ensure it's rendered after opaque objects

        // Get the initial color of the material
        Color originalColor = material.color;
        float elapsed = 0f;

        // Gradually fade out the alpha channel
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration); // Linear fade from 1 to 0
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure the object is fully transparent
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Destroy the object after fading out
        Destroy(obj);
    }

    public void FinalScore(int winCondition)
    {
        // Set 1st line of text based on winCondition
        if (winCondition == GAMELOST)
        {
            firstLineOfText.SetText("Game Over!");
        }
        else if (winCondition == GAMEWON)
        {
            firstLineOfText.SetText("You Won!");
        }
        else
        {
            Debug.Log("winCondition unknown; defaulting to failure.");
            Debug.Log("In the words of Stephen He: Emotional damage! Haha.");
            firstLineOfText.SetText("Failure! Emotional damage!");
        }

        // Show Game Over text and final score
        gameOverText1.SetActive(true);
        int finalScore = scoreManager.GetScore();
        scoreText.SetText($"Final Score: {finalScore}");
        finalScoreTextGO.SetActive(true);
        gameOverText2.SetActive(true);
        mainScoreTextGO.SetActive(false);

        // Start waiting for player input to restart
        StartCoroutine(WaitForRestart());
    }

    private IEnumerator WaitForRestart()
    {
        // Wait for any key press
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

}
