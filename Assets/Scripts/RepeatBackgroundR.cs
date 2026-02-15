using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackgroundR : MonoBehaviour
{
    // Vars
    private Vector3 startPos;
    private float repeatWidth;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        //repeatWidth = GetComponent<BoxCollider>().size.x / 2;
        repeatWidth = transform.localScale.x / 2;
    }

    // Update is called once per frame, on a fixed basis?
    // Kudos to Zeskos (1/16/25, on Twitch!)
    void FixedUpdate()
    {
        // how to repeat it when it is moving to the right?
        if (transform.position.x > startPos.x + repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
