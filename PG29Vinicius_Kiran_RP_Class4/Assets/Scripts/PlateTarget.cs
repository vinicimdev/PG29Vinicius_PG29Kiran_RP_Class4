using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlateTarget : MonoBehaviour
{
    [Header("Plate Settings")]
    [SerializeField] private Image ingredientFillImage; // Image that will fill (Filled type)

    [Header("Visual Feedback")]
    [SerializeField] private GameObject highlightEffect; // Optional: visual effect when dragging over

    [Header("Fill Settings")]
    [SerializeField] private float fillPerIngredient = 0.33f; // 33% per ingredient
    [SerializeField] private float fillSpeed = 0.5f; // Fill animation speed

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "NextLevel"; // Scene name to load when completed
    [SerializeField] private float delayBeforeLoadScene = 1f; // Delay before loading scene

    private int ingredientCount = 0;
    private const int maxIngredients = 3;
    private float targetFillAmount = 0f;

    void Start()
    {
        // Ensures the image is configured correctly
        if (ingredientFillImage != null)
        {
            // Configure image for Vertical Fill (Bottom)
            ingredientFillImage.type = Image.Type.Filled;
            ingredientFillImage.fillMethod = Image.FillMethod.Vertical;
            ingredientFillImage.fillOrigin = (int)Image.OriginVertical.Bottom;
            ingredientFillImage.fillAmount = 0f;
            targetFillAmount = 0f;
        }

        if (highlightEffect != null)
        {
            highlightEffect.SetActive(false);
        }
    }

    void Update()
    {
        // Smoothly animates the fill
        if (ingredientFillImage != null && ingredientFillImage.fillAmount != targetFillAmount)
        {
            ingredientFillImage.fillAmount = Mathf.Lerp(
                ingredientFillImage.fillAmount,
                targetFillAmount,
                Time.deltaTime * fillSpeed * 5
            );

            // Round when very close
            if (Mathf.Abs(ingredientFillImage.fillAmount - targetFillAmount) < 0.01f)
            {
                ingredientFillImage.fillAmount = targetFillAmount;
            }
        }
    }

    public void AddIngredient()
    {
        ingredientCount++;

        // Limit to maximum of 3 ingredients
        if (ingredientCount > maxIngredients)
        {
            ingredientCount = maxIngredients;
        }

        // Update fill amount (0.33, 0.66, 1.0)
        UpdateFillAmount();

        // Check if plate is completed
        if (ingredientCount >= maxIngredients)
        {
            OnPlateCompleted();
        }
    }

    private void UpdateFillAmount()
    {
        if (ingredientFillImage == null) return;

        // Calculate new fill amount
        targetFillAmount = ingredientCount * fillPerIngredient;

        // Ensure it doesn't exceed 1.0
        targetFillAmount = Mathf.Clamp01(targetFillAmount);

        Debug.Log($"Ingredient added! Fill: {targetFillAmount * 100}%");
    }

    private void OnPlateCompleted()
    {
        Debug.Log("Plate complete! Loading next scene...");

        // Load next scene after delay
        Invoke("LoadNextScene", delayBeforeLoadScene);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name not set!");
        }
    }

    public void ResetPlate()
    {
        ingredientCount = 0;
        targetFillAmount = 0f;

        if (ingredientFillImage != null)
        {
            ingredientFillImage.fillAmount = 0f;
        }
    }

    public int GetIngredientCount()
    {
        return ingredientCount;
    }

    public bool IsComplete()
    {
        return ingredientCount >= maxIngredients;
    }

    public float GetFillAmount()
    {
        return ingredientFillImage != null ? ingredientFillImage.fillAmount : 0f;
    }

    // Visual feedback when ingredient is over the plate
    void OnTriggerEnter2D(Collider2D other)
    {
        if (highlightEffect != null && other.GetComponent<DraggableIngredient>() != null)
        {
            highlightEffect.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (highlightEffect != null && other.GetComponent<DraggableIngredient>() != null)
        {
            highlightEffect.SetActive(false);
        }
    }
}