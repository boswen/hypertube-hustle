using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovePlayerLeft : MonoBehaviour
{
    // Vars
    private float leftBound = 0;

    // Reference to Player's MovePlayerLeft component
    private MovePlayerLeft runOffScreenTrigger;
    private Animator playerAnim; // to stop footstep sound events

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the Player for limiting actions based on gameover
        runOffScreenTrigger = GameObject.Find("Player").GetComponent<MovePlayerLeft>();
        playerAnim = GameObject.Find("Player").GetComponent <Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        // Stop movement of the player GO once it is out of bounds
        if (transform.position.x < leftBound)
        {
            runOffScreenTrigger.enabled = false;
            playerAnim.fireEvents = false;
        }

        // This way of doing things was chosen instead of a simpler DestroyObject
        // function since some game functions are built-in to the player script!
        // Future Nate: Put those functions elsewhere!
    }
}
