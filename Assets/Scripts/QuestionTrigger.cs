using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public bool isFinishPost = false;
    public string playerTag = "Player";

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color normalColor = Color.white;
    public Color triggeredColor = Color.yellow;

    private bool hasTriggered = false;

    void Start()
    {
        // Set initial color
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (isFinishPost)
        {
            HandleFinishPost();
        }
        else
        {
            HandleQuestionPost();
        }
    }

    void HandleQuestionPost()
    {
        if (hasTriggered) return;

        // Visual feedback
        if (spriteRenderer != null)
            spriteRenderer.color = triggeredColor;

        // Show random question
        if (QuestionManager.Instance != null)
        {
            QuestionManager.Instance.ShowRandomQuestion();
        }
        else
        {
            Debug.LogError("QuestionManager not found!");
        }

        hasTriggered = true;

        // Optional: Disable trigger after use
        // GetComponent<Collider2D>().enabled = false;
    }

    void HandleFinishPost()
    {
        // Handle finish logic here
        Debug.Log("Player reached finish!");

        // Visual feedback
        if (spriteRenderer != null)
            spriteRenderer.color = Color.green;

        // You can add finish game logic here
        // For example: show completion message, calculate score, etc.
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Reset color when player leaves (optional)
        if (spriteRenderer != null && !hasTriggered)
            spriteRenderer.color = normalColor;
    }

    // Method to reset trigger (useful for testing or reusing posts)
    public void ResetTrigger()
    {
        hasTriggered = false;
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;
    }
}