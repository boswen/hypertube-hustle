using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoUrl : MonoBehaviour
{
    [SerializeField] private string fileName = "Right to Left - Take 3.mp4";

    void Awake()
    {
        var vp = GetComponent<VideoPlayer>();
        // StreamingAssets is served as a web URL in WebGL
        vp.source = VideoSource.Url;
        vp.clip = null; // guarantees it is not treated as embedded for maximum Web GL compatibility
        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        // Optional: helpful defaults
        vp.playOnAwake = true;
        vp.isLooping = true;
    }
}
