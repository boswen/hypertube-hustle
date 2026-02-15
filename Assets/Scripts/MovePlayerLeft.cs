using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerLeft : MonoBehaviour
{
    // Vars
    public float travelSpeed = 12.0f;

    // Start is called before the first frame update
    void Start()
    {
        // N/A
    }

    // Update is called once per frame
    void Update()
    {
        // We could lock the player's y position so it doesn't fall below the screen
        // ... because why not... But it doesn't really make a difference either way.
        
        // Send player GameObject to the left; could probably combine with StopMovePlayerLeft()
        // class functions via a simple if statement ... (Future Nate: Make it so!)
        transform.Translate(Vector3.forward * Time.deltaTime * travelSpeed);
    }
}
