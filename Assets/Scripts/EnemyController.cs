using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Vars
    public ParticleSystem stompParticles;
    public AudioClip stompSound;

    private AudioSource audioSource;
    private MeshRenderer enemyRenderer;

    // Tweak these in the Inspector
    [SerializeField] private float squishDuration = 0.5f;  // how many seconds to squish
    [SerializeField] private float finalSquishScaleY = 0.1f;  // final scale on Y-axis
    [SerializeField] private float fadeDuration = 1.0f;   // how many seconds to fade out

    private void Start()
    {
        // If you have an AudioSource on the hog, cache it here
        audioSource = GetComponent<AudioSource>();

        // Grabs the first Renderer on this object or child object
        enemyRenderer = GetComponentInChildren<MeshRenderer>();

        // Make sure your material is set to a Fade/Transparent mode
        // so changing the alpha actually works visually.
        // For example, if you’re using the Standard Shader, set 
        // Rendering Mode to "Fade" or "Transparent" in the material’s Inspector.
    }

    public void OnStomped()
    {
        // Play particle effect
        if (stompParticles != null)
        {
            stompParticles.Play();
        }

        // Play sound effect
        if (stompSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(stompSound);
        }

        // Start the squish & fade-out sequence
        StartCoroutine(SquishAndFade());
    }

    private IEnumerator SquishAndFade()
    {
        // 1) "Squish" enemy by scaling down height
        Vector3 originalScale = transform.localScale;
        Vector3 finalScale = new Vector3(originalScale.x, finalSquishScaleY, originalScale.z);

        float elapsed = 0f;
        while (elapsed < squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / squishDuration);

            // Smoothly interpolate the scale along Y
            transform.localScale = Vector3.Lerp(originalScale, finalScale, t);

            yield return null;
        }

        // 2) Fade out enemy mesh
        if (enemyRenderer != null && enemyRenderer.material != null)
        {
            // Switch to Fade mode (only once)
            SetMaterialToFadeMode(enemyRenderer.material);

            Color originalColor = enemyRenderer.material.color;
            float alphaElapsed = 0f;

            while (alphaElapsed < fadeDuration)
            {
                alphaElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(alphaElapsed / fadeDuration);

                float newAlpha = Mathf.Lerp(1f, 0f, t);
                enemyRenderer.material.color = new Color(originalColor.r, originalColor.g,
                                                         originalColor.b, newAlpha);

                yield return null;
            }
        }

        // 3) Remove game object from world
        Destroy(gameObject);
    }

    /// <summary>
    /// Forces a standard Unity material (using the built-in "Standard" shader)
    /// into a Fade mode so that changing the alpha actually makes the model transparent.
    /// </summary>
    private void SetMaterialToFadeMode(Material mat)
    {
        // This is the built-in approach for Unity’s Standard Shader
        // to switch to Transparent/Fade at runtime.

        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
}
