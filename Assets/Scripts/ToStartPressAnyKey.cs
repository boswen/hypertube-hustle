using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToStartPressAnyKey : MonoBehaviour
{
    // Vars
    //private AudioListener mainCameraEars; // for disabling all sound during title
    // ... except we can't just turn off the ears or Unity will get mad and proudce a
    // warning message: "There are no audio listeners in the scene. Please ensure there
    // is always one audio listener in the scene." Soooooo ... let's just get the
    // references to the sources of these sounds and disable them *that* way instead!
    private GameObject bgMusicGO; // for disabling background music

    // We'll need references to a few other things as well
    private SpawnManager spawner;
    private GameObject titleScreen;
    private GameObject backgroundMovie;
    //private Animator playerAnim;
    private AudioSource playerAudioSource;
    //private ParticleSystem dirtParticle;
    private GameObject dirtParticle;
    public GameObject mainScoreTextGO; // set in inspector

    public bool hasStarted = false;
    public PlayerController playerCtrlRef;

    // Start is called before the first frame update
    void Start()
    {
        // Setup Component/GO "pointers" (sort of)
        spawner = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        //mainCameraEars = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        bgMusicGO = GameObject.Find("BackgroundMusic");
        titleScreen = GameObject.Find("Title Screen");
        backgroundMovie = GameObject.Find("BG Movie");
        playerAudioSource = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        dirtParticle = GameObject.Find("FX_DirtSplatter");
        //mainScoreTextGO = GameObject.Find("Score");
        playerCtrlRef = GameObject.Find("Player").GetComponent<PlayerController>();

        // Then use those psuedo-pointers to set the game environment to "title" screen mode
        // --Disable main game spawner (in case it isn't already, e.g. on a restart)
        //spawner.enabled = false; // seems like this isn't needed.
        // --Enable the title screen (in csae it was off)
        titleScreen.SetActive(true);
        // --Pssst! Camera! Plug your ears for a bit! ;) ^^
        // ...(e.g., ensure sound like footsteps + music is off until StartGame() is called!)
        //mainCameraEars.enabled = false;
        // ---cough ... Nevermind. Let's turn off the sources instead
        bgMusicGO.SetActive(false); // background music
        playerAudioSource.enabled = false; // disable sound of running footsteps
        // --Keep the dirt particles off until the game starts
        dirtParticle.SetActive(false);
        // --Disable the score text for now
        mainScoreTextGO.SetActive(false);

        // --Restart the background movie by toggling the GO off and then back on
        backgroundMovie.SetActive(false);
        backgroundMovie.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // listen for any input and call spawner.StartGame() once received
        if (!hasStarted && Input.anyKeyDown)
        {
            hasStarted = true; // set it to true in case we need to track this later
            playerCtrlRef.isThisThingOn = true;

            // It's "Go" time!!
            // --Hide Title UI
            titleScreen.SetActive(false);
            // --Start the main game
            //spawner.enabled = true; // unnecessary .. just wait to do the below
            spawner.StartGame();
            // --"Unplug" the ears on the camera so that sound works again! :D
            //mainCameraEars.enabled = true;
            // ---*Cough* Apparently, we need to turn on the sound sources instead.
            bgMusicGO.SetActive(true); // background music
            playerAudioSource.enabled = true; // re-enable sound of running footsteps
            // --Next, keep the dirt particles off until the game starts
            dirtParticle.SetActive(true);
            // --Re-enable the score text
            mainScoreTextGO.SetActive(true);
            // --Restart the background movie again to prep for gameplay!
            backgroundMovie.SetActive(false);
            backgroundMovie.SetActive(true);
        }
    }
}
