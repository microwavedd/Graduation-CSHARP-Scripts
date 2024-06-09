using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MediaLoader : MonoBehaviour
{
    public Transform contentParent; // Parent transform to hold the media thumbnails
    public GameObject imagePrefab;  // Prefab for displaying images
    public GameObject videoPrefab;  // Prefab for displaying videos

    private string folderPath;

    void Start()
    {
        folderPath = Path.Combine(Application.dataPath, "Images");
        LoadMediaFiles();
    }

    void LoadMediaFiles()
    {
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                if (IsImageFile(file))
                {
                    CreateImageThumbnail(file);
                }
                else if (IsVideoFile(file))
                {
                    CreateVideoThumbnail(file);
                }
            }
        }
        else
        {
            Debug.LogError("Folder not found: " + folderPath);
        }
    }

    bool IsImageFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".png" || extension == ".jpg" || extension == ".jpeg";
    }

    bool IsVideoFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".mp4" || extension == ".mov" || extension == ".avi";
    }

    void CreateImageThumbnail(string filePath)
    {
        GameObject imageObject = Instantiate(imagePrefab, contentParent);
        Image imageComponent = imageObject.GetComponent<Image>();
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void CreateVideoThumbnail(string filePath)
    {
        GameObject videoObject = Instantiate(videoPrefab, contentParent);
        RawImage rawImage = videoObject.GetComponent<RawImage>();
        VideoPlayer videoPlayer = videoObject.GetComponent<VideoPlayer>();

        videoPlayer.url = filePath;
        videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += (source) => {
            RenderTexture renderTexture = new RenderTexture((int)videoPlayer.width, (int)videoPlayer.height, 0);
            videoPlayer.targetTexture = renderTexture;
            rawImage.texture = renderTexture;
            videoPlayer.Play();
        };
        videoPlayer.Prepare();

        // Add a click listener to stop the video when clicked
        Button videoButton = videoObject.AddComponent<Button>();
        videoButton.onClick.AddListener(() => {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        });
    }
}

