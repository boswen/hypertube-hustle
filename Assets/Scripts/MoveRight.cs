using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : MonoBehaviour
{
    // Vars
    public float travelSpeed = 12.0f; 
    private PlayerController playerControllerScript;
    private float rightBound = 50; // x-val where avoided obstacles + missed pickups are now a good ways off-screen

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only move an object if gameOver hasn't happened!
        if (playerControllerScript.gameOver == false)
        {
            // Send object or background to the left
            transform.Translate(Vector3.right * Time.deltaTime * travelSpeed);
        }

        // Destroy all objects that move out of bounds of the camera if they are NOT the background
        if (transform.position.x > rightBound && !gameObject.CompareTag("Background"))
        {
            Destroy(gameObject);
        }
    }
}
