using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroundRight : MonoBehaviour
{
    // Vars
    private Vector3 startPos;
    private float repeatWidth;

    public float travelSpeed = 12.0f;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        // Method 1: Calculate width based on child renderers
        repeatWidth = CalculateTotalWidth() / 2;

        Debug.Log("Calculated repeat width: " + repeatWidth);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Calculate the total width of all child renderers
    private float CalculateTotalWidth()
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        // Get all renderers (SpriteRenderer or MeshRenderer) in children
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Get the bounds in world space
            Bounds bounds = renderer.bounds;

            // Update min and max X positions
            minX = Mathf.Min(minX, bounds.min.x);
            maxX = Mathf.Max(maxX, bounds.max.x);
        }

        // Return the total width
        return maxX - minX;
    }

    // Update is called once per frame
    // Kudos to Zeskos (1/16/25, on Twitch!)
    void FixedUpdate()
    {
        // Only move an object if gameOver hasn't happened!
        if (playerControllerScript.gameOver == false)
        {
            // Send object or background to the right
            transform.Translate(Vector3.right * Time.deltaTime * travelSpeed);
        }

        // Repeat it once it has moved half way to the right
        if (transform.position.x > startPos.x + repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
