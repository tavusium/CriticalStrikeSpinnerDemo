
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class splash_videoplayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextScene = "scene_game_mainscreen";

    private void Awake()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer player)
    {
        SceneManager.LoadScene(nextScene);
    }
}
