using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCollectibles : MonoBehaviour
{
    // vars
    public float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // N/A
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
