using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoPlayerController : MonoBehaviour
{
    public List<VideoPlayer> videoPlayers;
    private int currentIndex = 0;

    void Start()
    {
        InitializeVideoPlayers();
        DisplayCurrentVideoPlayer();
    }

    void InitializeVideoPlayers()
    {
        foreach (var player in videoPlayers)
        {
            player.Stop();
            player.gameObject.SetActive(false);
        }
    }

    void DisplayCurrentVideoPlayer()
    {
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            if (i == currentIndex)
            {
                videoPlayers[i].gameObject.SetActive(true);
                videoPlayers[i].Play();
            }
            else
            {
                videoPlayers[i].gameObject.SetActive(false);
                videoPlayers[i].Stop();
            }
        }
    }

    public void NextVideo()
    {
        currentIndex++;
        if (currentIndex >= videoPlayers.Count)
        {
            currentIndex = 0;
        }
        DisplayCurrentVideoPlayer();
    }

    public void PreviousVideo()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = videoPlayers.Count - 1;
        }
        DisplayCurrentVideoPlayer();
    }
}
