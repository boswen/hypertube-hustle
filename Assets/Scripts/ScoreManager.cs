using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices; // For TextMeshPro

public class ScoreManager : MonoBehaviour
{
    // Vars
    public GameObject scoreTextGO; // Assign the game object in the Inspector
    private TextMeshProUGUI scoreText; // Auto-Assigned ScoreText UI from above
    public float timeMultiplier = 1.0f; // Points per second alive
    public int avoidBonus = 3; // Points per avoided obstacle
    public int pickupBonus = 5; // Points per object pickup
    public int squishBonus = 7; // Points per object pickup

    public float elapsedTime = 0f; // Tracks how long the player has survived
    public int timeScore = 0; // Points from time
    public int bonusScore = 0; // Points from obstacle avoidance and pickup bonuses
    public int totalScore = 0; // Combined score (time + bonuses)

    private PlayerController playerControllerScript;
    private bool gameOver = false;
    private bool isThisThingyOn;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        scoreText = scoreTextGO.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        gameOver = playerControllerScript.gameOver;
        isThisThingyOn = playerControllerScript.isThisThingOn;

        if (gameOver) return;

        // Increment elapsed time
        if (isThisThingyOn)
        {
            elapsedTime += Time.deltaTime;
        }

        // Calculate time-based score
        timeScore = Mathf.FloorToInt(elapsedTime * timeMultiplier);

        // Check for obstacle avoidance
        AddAvoidBonus();

        // Bonus pickup function "AddPickupBonus()" is called by PlayerControllerX

        // Combine scores
        totalScore = timeScore + bonusScore;

        // Update the UI
        if (isThisThingyOn)
        {
            UpdateScoreText();
        }
    }

    public void AddAvoidBonus()
    {
        if (gameOver) return;

        // Add bonus points for avoiding an obstacle
        // Prototype: For now, just add the bonus anytime the jump button is pressed... Later: implement ground checks

        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            bonusScore += avoidBonus;
        }

        // Update the total score and UI
        totalScore = timeScore + bonusScore;
        UpdateScoreText();
    }

    public void AddPickupBonus()
    {
        if (gameOver) return;

        // Add bonus points for collecting the $ items
        // logic implemented via collision check in the PlayerController script
        bonusScore += pickupBonus;

        // Update the total score and UI
        totalScore = timeScore + bonusScore;
        UpdateScoreText();
    }

    public void AddSquishBonus()
    {
        if (gameOver) return;

        // Add bonus points for squishing the enemies
        // logic implemented via collision check in the PlayerFootCollider script
        bonusScore += squishBonus;

        // Update the total score and UI
        totalScore = timeScore + bonusScore;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return totalScore;
    }

    private void UpdateScoreText()
    {
        if (!gameOver)
        {
            scoreText.text = $"Score: {totalScore}";
        }
    }
}
