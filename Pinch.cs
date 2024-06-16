using UnityEngine;
using System.Collections.Generic;

public class PinchToZoom : MonoBehaviour
{
    public float minScale = 1f;
    public float maxScale = 5f;
    public float zoomSpeed = 0.1f;
    public List<GameObject> uiElementsToHide;

    private RectTransform rectTransform;
    private Vector2 prevTouchDelta;
    private Vector2 currentTouchDelta;
    private bool isZooming = false;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                prevTouchDelta = touch1.position - touch2.position;
                isZooming = true;
                HideUIElements();
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                currentTouchDelta = touch1.position - touch2.position;
                float prevMagnitude = prevTouchDelta.magnitude;
                float currentMagnitude = currentTouchDelta.magnitude;
                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * zoomSpeed);
                prevTouchDelta = currentTouchDelta;
            }
        }
        else if (Input.touchCount == 1 && rectTransform.localScale.x > minScale)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;
                Pan(touchDeltaPosition);
            }
        }
        else
        {
            isZooming = false;
        }

        if (!isZooming && rectTransform.localScale.x <= minScale)
        {
            rectTransform.localPosition = originalPosition;
            ShowUIElements();
        }
        else
        {
            rectTransform.localPosition = ClampPosition(rectTransform.localPosition);
        }
        if (rectTransform.localScale.x == minScale && rectTransform.localScale.y == minScale)
        {
            ShowUIElements();
        }
        else
        {
            HideUIElements();
        }
    }

    private void Zoom(float increment)
    {
        Vector3 scale = rectTransform.localScale;
        scale += Vector3.one * increment;
        scale = ClampScale(scale);
        rectTransform.localScale = scale;

        rectTransform.localPosition = ClampPosition(rectTransform.localPosition);
    }

    private Vector3 ClampScale(Vector3 scale)
    {
        scale.x = Mathf.Clamp(scale.x, minScale, maxScale);
        scale.y = Mathf.Clamp(scale.y, minScale, maxScale);
        scale.z = 1;
        return scale;
    }

    private void Pan(Vector2 deltaPosition)
    {
        Vector3 newPosition = rectTransform.localPosition + new Vector3(deltaPosition.x, deltaPosition.y, 0);
        rectTransform.localPosition = ClampPosition(newPosition);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        float halfWidth = rectTransform.rect.width * (rectTransform.localScale.x - 1) / 2;
        float halfHeight = rectTransform.rect.height * (rectTransform.localScale.y - 1) / 2;

        float clampedX = Mathf.Clamp(position.x, -halfWidth, halfWidth);
        float clampedY = Mathf.Clamp(position.y, -halfHeight, halfHeight);

        return new Vector3(clampedX, clampedY, position.z);
    }

    private void HideUIElements()
    {
        foreach (var uiElement in uiElementsToHide)
        {
            uiElement.SetActive(false);
        }
    }

    private void ShowUIElements()
    {
        foreach (var uiElement in uiElementsToHide)
        {
            uiElement.SetActive(true);
        }
    }

    public void ResetZoom()
    {
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = originalPosition;
        ShowUIElements();
    }
}
