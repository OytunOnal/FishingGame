using DG.Tweening;
using UnityEngine;

public class FishAnimator : MonoBehaviour
{
    // Shake effect settings: control randomization ranges for shake duration and strength
    private readonly float minShakeDuration = 0.1f, maxShakeDuration = 0.25f;
    private readonly float minShakeStrength = 0.5f, maxShakeStrength = 1f;
    private readonly int minShakeVibrato = 5, maxShakeVibrato = 10;

    // Fade effect setting: target alpha for fade effect
    private readonly float fadeAmount = 0.25f;

    // Tween objects to control fade and shake animations
    private Tween fadeTween;
    private Tween shakeTween;

    // References to material and animator for controlling visual and animation effects
    private Material material;
    private Animator fishAnimator;

    private void Awake()
    {
        // Initialize references to fish's material and animator
        material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        fishAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Initiates a randomized shake and fade animation on the fish.
    /// </summary>
    internal void ShakeAndFade()
    {
        // Randomly determine shake properties within specified ranges
        float randomShakeDuration = Random.Range(minShakeDuration, maxShakeDuration);
        float randomShakeStrength = Random.Range(minShakeStrength, maxShakeStrength);
        int randomShakeVibrato = Random.Range(minShakeVibrato, maxShakeVibrato);

        StopShakeAndFade();
        // Apply the shake effect, shaking the fish model's scale with a randomized intensity
        shakeTween = transform.GetChild(0)
            .DOShakeScale(randomShakeDuration, randomShakeStrength, randomShakeVibrato, 90, false);

        // Fade the material alpha to create a fade in/out effect
        fadeTween = material
            .DOFade(fadeAmount, randomShakeDuration)
            .SetLoops(2, LoopType.Yoyo); // Loops the fade to fade in and back out
    }

    /// <summary>
    /// Completes any active shake and fade animations immediately.
    /// </summary>
    internal void StopShakeAndFade()
    {
        fadeTween?.Complete();
        shakeTween?.Complete();
    }

    /// <summary>
    /// Sets the animation speed for the fish animator.
    /// </summary>
    /// <param name="newSpeed">New speed value to apply to the animator.</param>
    internal void SetSpeed(float newSpeed)
    {
        fishAnimator.speed = newSpeed;
    }
}
