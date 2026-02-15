using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    // Vars
    public TextMeshProUGUI pressStartText;
    public float blinkInterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            pressStartText.enabled = !pressStartText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // N/A
    }
}
