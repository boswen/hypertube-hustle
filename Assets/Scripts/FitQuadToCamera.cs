using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitQuadToCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void LateUpdate()
    {
        if (!cam) cam = Camera.main;
        if (!cam || !cam.orthographic) return;

        float h = cam.orthographicSize * 2f;
        float w = h * cam.aspect;
        transform.localScale = new Vector3(w, h, 1f);
    }
}

