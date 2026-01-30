using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private float dragAlpha = 0.7f; // Transparency while dragging

    private RectTransform rectTransform;
    private Image image;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Color originalColor;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        // Save original color
        if (image != null)
        {
            originalColor = image.color;
        }

        // Find Canvas automatically if not set
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save original position
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        // Place in front of other objects
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        // Make semi-transparent while dragging
        if (image != null)
        {
            Color tempColor = image.color;
            tempColor.a = dragAlpha;
            image.color = tempColor;

            // Disable raycast to not block plate detection
            image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move ingredient following cursor/finger
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore opacity and raycast
        if (image != null)
        {
            image.color = originalColor;
            image.raycastTarget = true;
        }

        // Check if dropped on top of a plate
        PlateTarget plate = GetPlateUnderPointer(eventData);

        if (plate != null)
        {
            // Add ingredient to plate and destroy this object
            plate.AddIngredient();
            Destroy(gameObject);
        }
        else
        {
            // Return to original position
            ReturnToOriginalPosition();
        }
    }

    private PlateTarget GetPlateUnderPointer(PointerEventData eventData)
    {
        // Raycast to see what's under the pointer
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            PlateTarget plate = result.gameObject.GetComponent<PlateTarget>();
            if (plate != null)
            {
                return plate;
            }
        }

        return null;
    }

    private void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
    }
}