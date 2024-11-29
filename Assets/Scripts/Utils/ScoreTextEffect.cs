using UnityEngine;
using DG.Tweening;

public class ScoreTextEffect : MonoBehaviour
{
    private TextMesh scoreText;         // Using TextMesh for text display
    private float fadeDuration = 1.75f;  // Duration for fade out
    private float moveDistance = 2.5f;  // Distance to move up

    private void Awake()
    {
        scoreText = GetComponent<TextMesh>();
    }

    /// <summary>
    /// Plays the score effect with the given score, starting at the current position.
    /// </summary>
    /// <param name="score">The score value to display.</param>
    public void PlayEffect(int score)
    {
        scoreText.text = $"+{score}";

        // Set initial color alpha to fully visible
        Color startColor = scoreText.color;
        startColor.a = 1f;
        scoreText.color = startColor;

        // Create a sequence for the move and fade animations
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(transform.position.y + moveDistance, fadeDuration))
                .Join(DOFadeTextMeshAlpha(0, fadeDuration));  // Custom method to animate alpha
                //.OnComplete(() => PoolManager.Despawn(gameObject)); // Return to pool after animation
    }

    /// <summary>
    /// Resets the alpha when the object is enabled to make it fully visible.
    /// </summary>
    private void OnEnable()
    {
        Color color = scoreText.color;
        color.a = 1f;
        scoreText.color = color;
        PlayEffect(3);
    }

    /// <summary>
    /// Custom method to animate the alpha of the TextMesh color.
    /// </summary>
    private Tween DOFadeTextMeshAlpha(float targetAlpha, float duration)
    {
        return DOTween.To(() => scoreText.color.a, x => {
            Color c = scoreText.color;
            c.a = x;
            scoreText.color = c;
        }, targetAlpha, duration);
    }
}
