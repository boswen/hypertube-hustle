//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.SceneManagement;
//using UnityEngine;

//public class PlayerFootCollider : MonoBehaviour
//{
//    // Vars
//    private Rigidbody playerRb;
//    private Animator playerAnim;
//    private Vector3 playerVelocity;

//    private readonly float stompBounceForce = 500.0f;

//    private EnemyController enemyController;
//    private ScoreManager scoreManager;

//    private PlayerAudioController playerAudioController;
//    private PlayerController playerController;

//    // Track whether the player is currently considered "in the air"
//    private bool isAirborne = true;

//    // Start is called before the first frame update
//    void Start()
//    {
//        playerRb = GetComponentInParent<Rigidbody>();
//        playerAnim = GetComponentInParent<Animator>();
//        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

//        // Get the audio controller from the parent (assuming it's on the same Player object)
//        playerAudioController = GetComponentInParent<PlayerAudioController>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        playerVelocity = playerRb.velocity;
//    }

//    // Process foot collider's collision with something.
//    void OnCollisionEnter(Collision collision)
//    {
//        Debug.Log("Player's foot collider collided with" + collision.gameObject.name);

//        // Enemy Stomp
//        if (collision.gameObject.CompareTag("EnemyTop"))
//        {
//            // Confirm we're moving downward
//            if (playerVelocity.y < 0)
//            {
//                // Bounce the player
//                playerRb.AddForce(Vector3.up * stompBounceForce, ForceMode.Impulse);
//                playerAnim.SetTrigger("Jump_trig");
//                // Bounce sound and "squish" particle effects are played via enemy controller script

//                // Add squish points!
//                scoreManager.AddSquishBonus(); // add points!

//                // Tell enemy it got stomped
//                enemyController = collision.gameObject.GetComponentInParent<EnemyController>();
//                enemyController.OnStomped();

//                // We remain "in the air" after a bounce, so don't set isAirborne = false here
//                // Instead, the main playerController will register the collision with ground
//                // and set it's own isOnGround bool to be true.
//            }
//        }

//        // Parkour landing
//        if (collision.gameObject.CompareTag("ObstacleTop"))
//        {
//            // Confirm we're moving downward; otherwise we "missed the jump" and the main collider for the
//            // player body should hit the obstacle and existing logic should trigger gameover
//            if (playerVelocity.y < 0)
//            {
//                // If we were in the air, then just land and run across the
//                // top of the barrier like any other flat surface...
//                if (isAirborne)
//                {
//                    // Play the landing sound
//                    if (playerAudioController != null)
//                    {
//                        playerAudioController.Land();
//                    }

//                    // Award parkour bonus (reusing the squish method for now)
//                    scoreManager.AddSquishBonus();

//                    // Mark that we're no longer airborne
//                    isAirborne = false;

//                    // If we're already on top of the obstacle and just continuing to collide,
//                    // we won�t re-run this land logic unless we jump again and come back down.
//                    // No bounce effect, bounce sound, or "squish" particle effects need to be
//                    // played since concrete barriers don't "squish" or bounce us. Ergo, no
//                    // need to tell the obstacle it got stomped either...
//                }
//            }
//        }

//    }

//    // This is optional, but you might detect leaving the obstacle or ground.
//    // If the player jumps again, set isAirborne = true.
//    void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("ObstacleTop") || collision.gameObject.CompareTag("Ground"))
//        {
//            // The foot collider is no longer in contact with the ground or obstacle
//            isAirborne = true;
//            playerController.isOnGround = false;
//        }
//    }
//}
