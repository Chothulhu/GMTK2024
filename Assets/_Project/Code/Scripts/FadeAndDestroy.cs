using System.Collections;
using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    public float fadeDuration = 5f; // Duration of the fade in seconds

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            StartCoroutine(FadeOutAndDestroy());
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found on the GameObject.");
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure the sprite is fully transparent
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
