using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotoGallery : MonoBehaviour
{
    public Image photoDisplay; 
    public Sprite[] photos; 
    public GameObject[] photoDescriptions1; 
    public GameObject[] photoDescriptions2; 
    private int currentIndex = 0; 

    private const float maxWidth = 400f;
    private const float maxHeight = 800f;

    void Start()
    {
        // Validate photoDisplay
        if (photoDisplay == null)
        {
            Debug.LogError("Photo display is not assigned.");
            return;
        }

        // Validate arrays
        if (photos == null || photos.Length == 0)
        {
            Debug.LogError("Photos array is not assigned or is empty.");
            return;
        }
        if (photoDescriptions1 == null || photoDescriptions1.Length != photos.Length)
        {
            Debug.LogError("PhotoDescriptions1 array is not assigned or its length does not match the photos array.");
            return;
        }
        if (photoDescriptions2 == null || photoDescriptions2.Length != photos.Length)
        {
            Debug.LogError("PhotoDescriptions2 array is not assigned or its length does not match the photos array.");
            return;
        }
        photoDisplay.gameObject.SetActive(false);

        DisplayPhoto(currentIndex); 
    }

    public void ShowNextPhoto()
    {
        if (photos.Length == 0) return;

        currentIndex++;
        if (currentIndex >= photos.Length)
        {
            currentIndex = 0; 
        }
        DisplayPhoto(currentIndex);
    }

    public void ShowPreviousPhoto()
    {
        if (photos.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = photos.Length - 1; 
        }
        DisplayPhoto(currentIndex);
    }

    private void DisplayPhoto(int index)
    {
        if (photos.Length == 0 || index < 0 || index >= photos.Length)
        {
            Debug.LogWarning("Invalid index for displaying photo: " + index);
            return;
        }

        Sprite photo = photos[index];
        if (photo != null)
        {
            photoDisplay.sprite = photo;
            photoDisplay.SetAllDirty();

            if (!photoDisplay.gameObject.activeSelf)
            {
                photoDisplay.gameObject.SetActive(true);
            }
            RectTransform rt = photoDisplay.GetComponent<RectTransform>();
            float photoAspectRatio = (float)photo.texture.width / photo.texture.height;

            if (photo.texture.width > photo.texture.height)
            {
                float newWidth = Mathf.Min(maxWidth, maxHeight * photoAspectRatio);
                float newHeight = newWidth / photoAspectRatio;
                rt.sizeDelta = new Vector2(newWidth, newHeight);
            }
            else
            {
                float newHeight = Mathf.Min(maxHeight, maxWidth / photoAspectRatio);
                float newWidth = newHeight * photoAspectRatio;
                rt.sizeDelta = new Vector2(newWidth, newHeight);
            }
        }
        else
        {
            Debug.LogWarning("Photo at index " + index + " is null.");
        }

        if (index >= 0 && index < photoDescriptions1.Length && index < photoDescriptions2.Length)
        {
            for (int i = 0; i < photoDescriptions1.Length; i++)
            {
                photoDescriptions1[i].SetActive(i == index);
                photoDescriptions2[i].SetActive(i == index);
            }
        }
        else
        {
            Debug.LogError("Index out of range for photo descriptions: " + index);
        }
    }
}
