using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private bool _consumed; // flag to prevent multiple collisions/triggers from triggering the sequence multiple times

    public void ConsumeAllHitboxes()
    {
        if (_consumed) return;
        _consumed = true;

        foreach (var c in GetComponentsInChildren<Collider>(true))
            c.enabled = false;
    }

    // Called when player lands on top successfully
    public void OnLandedOnTop()
    {
        // If sequence has already been triggered, ignore subsequent calls (e.g. from multiple colliders)
        if (_consumed) return;

        // Optional: award score / play sfx / show particles
        // ...

        ConsumeAllHitboxes(); // IMPORTANT: prevents body collider game-over after landing
    }

    // Called when player hits the barrier body
    public void OnSideHit()
    {
        // If sequence has already been triggered, ignore subsequent calls (e.g. from multiple colliders)
        if (_consumed) return;

        ConsumeAllHitboxes(); // IMPORTANT: prevents top trigger firing after death
    }
}
