using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    //vars
    public AudioSource audioSource;     // assign source #2 in inspector
    public AudioClip[] footStepClips;   // assign in inspector
    public AudioClip[] jumpClips;       // assign in inspector
    public AudioClip[] landingClips;    // assign in inspector

    //constants
    private const int FOOTSTEPS = 1;
    private const int JUMPS = 2;
    private const int LANDINGS = 3;

    private void Awake()
    {
        //audioSource = GetComponent<AudioSource>();

        // Ensure footsteps don’t auto-play
        audioSource.playOnAwake = false;
    }

    //called by Animation controller at footstep events
    public void Step()
    {
        if (audioSource.enabled == true)
        {
            AudioClip clip = GetRandomClip(FOOTSTEPS);
            audioSource.PlayOneShot(clip);
        }
    }

    //called by Animation controller at jump events
    public void Jump()
    {
        AudioClip clip = GetRandomClip(JUMPS);
        audioSource.PlayOneShot(clip);
    }

    //called by Animation controller at landing events
    public void Land()
    {
        if (audioSource.enabled == true)
        {
            AudioClip clip = GetRandomClip(LANDINGS);
            audioSource.PlayOneShot(clip);
        }

        // FUTURE NATE: Set a state to allow footstep sounds again?
        // e.g., StartCoroutine(PlayFootstepSounds());
    }

    private AudioClip GetRandomClip(int typeOfSound)
    {
        switch (typeOfSound)
        {
            case FOOTSTEPS:
                return footStepClips[UnityEngine.Random.Range(0, footStepClips.Length)];
            case JUMPS:
                return jumpClips[UnityEngine.Random.Range(0, jumpClips.Length)];
            case LANDINGS:
                return landingClips[UnityEngine.Random.Range(0, landingClips.Length)];
            default:
                return footStepClips[UnityEngine.Random.Range(0, footStepClips.Length)];
        }

    }
}
