using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement; // Required to change scenes

public class VideoToScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneName = "Level2"; // CHANGE THIS to your actual scene name

    void Start()
    {
        // Auto-grab the component if you forgot to drag it in
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // IMPORTANT: The video must NOT loop, or it never technically "ends"
        videoPlayer.isLooping = false;

        // Subscribe to the "End of Video" event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    // This function runs automatically when the video ends
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished. Loading next scene...");
        SceneManager.LoadScene(sceneName);
    }
}