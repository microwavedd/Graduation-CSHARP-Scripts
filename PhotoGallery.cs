using UnityEngine;
using UnityEngine.UI;

public class PhotoGallery : MonoBehaviour
{
    public Image photoDisplay; // Reference to the UI Image component
    public Sprite[] photos; // Array of photos (sprites)
    private int currentIndex = 0; // Current photo index

    private const float maxWidth = 400f;
    private const float maxHeight = 800f;

    private PinchToZoom pinchToZoom;

    void Start()
    {
        if (photos.Length > 0)
        {
            DisplayPhoto(currentIndex); // Display the first photo
        }
        pinchToZoom = photoDisplay.GetComponent<PinchToZoom>();
    }

    public void ShowNextPhoto()
    {
        if (photos.Length == 0) return;

        currentIndex++;
        if (currentIndex >= photos.Length)
        {
            currentIndex = 0; // Loop back to the first photo
        }
        DisplayPhoto(currentIndex);
    }

    public void ShowPreviousPhoto()
    {
        if (photos.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = photos.Length - 1; // Loop back to the last photo
        }
        DisplayPhoto(currentIndex);
    }

    private void DisplayPhoto(int index)
    {
        Sprite photo = photos[index];
        photoDisplay.sprite = photo;

        // Adjust the size of the photoDisplay RectTransform to maintain the aspect ratio
        RectTransform rt = photoDisplay.GetComponent<RectTransform>();
        float photoAspectRatio = (float)photo.texture.width / photo.texture.height;

        if (photo.texture.width > photo.texture.height)
        {
            // Photo is wider than it is tall
            float newWidth = Mathf.Min(maxWidth, maxHeight * photoAspectRatio);
            float newHeight = newWidth / photoAspectRatio;
            rt.sizeDelta = new Vector2(newWidth, newHeight);
        }
        else
        {
            // Photo is taller than it is wide or has equal width and height
            float newHeight = Mathf.Min(maxHeight, maxWidth / photoAspectRatio);
            float newWidth = newHeight * photoAspectRatio;
            rt.sizeDelta = new Vector2(newWidth, newHeight);
        }

        // Reset the zoom when displaying a new photo
        if (pinchToZoom != null)
        {
            pinchToZoom.ResetZoom();
        }
    }
}
