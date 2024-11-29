using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private float startTimeInSeconds = 60f; // Default timer duration in seconds
    private TMP_Text timerText;
    private Timer timer;

    // Tween object to handle scale animation
    private Tween scaleTween;

    private void Awake()
    {
        // Get the TMP_Text component to update the timer display
        timerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (timer == null) return;

        // Retrieve and update the time in minutes and seconds format
        var (minutes, seconds) = timer.GetTime();
        UpdateTimerText(minutes, seconds);
    }

    /// <summary>
    /// Sets and initializes the timer with a specified duration.
    /// </summary>
    /// <param name="newStartTimeInSeconds">The duration to set for the timer.</param>
    public void SetTimer(float newStartTimeInSeconds)
    {
        startTimeInSeconds = newStartTimeInSeconds;

        if (timer == null)
            timer = new Timer(startTimeInSeconds, OnTimeOut);
        else
            timer.SetTimer(startTimeInSeconds);

        ResetTimeRunningOutEffects();
    }

    /// <summary>
    /// Starts the timer with a specified duration, initializing if necessary.
    /// </summary>
    /// <param name="durationInSeconds">The duration for which the timer will run.</param>
    public void StartTimer(float durationInSeconds)
    {
        SetTimer(durationInSeconds);
        timer.StartTimer();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        AudioManager.Instance.StopLoopedEffect();
        timer.StopTimer();
    }

    /// <summary>
    /// Restarts the timer from the initial start time.
    /// </summary>
    public void RestartTimer()
    {
        ResetTimer();
        timer.StartTimer();
    }

    /// <summary>
    /// Resets the timer to its initial duration.
    /// </summary>
    public void ResetTimer()
    {
        timer.SetTimer(startTimeInSeconds);
        ResetTimeRunningOutEffects();
    }

    /// <summary>
    /// Updates the timer text display and triggers a scale animation when 20 seconds remain.
    /// </summary>
    /// <param name="minutes">Minutes remaining on the timer.</param>
    /// <param name="seconds">Seconds remaining on the timer.</param>
    private void UpdateTimerText(int minutes, int seconds)
    {
        // Trigger scale animation and color change when 20 seconds remain
        if (minutes == 0 && seconds == 20)
        {
            timerText.color = Color.red;

            if (scaleTween == null)
            {
                AudioManager.Instance.PlayLoopedEffect(SoundEffectType.TickTock);
                // Start a looped scale tween effect on the timer text
                scaleTween = timerText.transform.DOScale(1.2f, 0.5f)
                    .SetLoops(-1, LoopType.Yoyo) // Loop animation back and forth
                    .SetEase(Ease.InOutSine); // Smooth in-out easing for scale animation
            }
        }

        // Update the timer text in MM:SS format
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Resets the scale animation and color as well as sound when the timer is reset or stopped.
    /// </summary>
    public void ResetTimeRunningOutEffects()
    {
        AudioManager.Instance.StopLoopedEffect();
        scaleTween?.Kill(); // Safely kill any active tween
        scaleTween = null; // Reset tween reference
        timerText.color = Color.white; // Reset text color to default
    }

    /// <summary>
    /// Callback function when the timer reaches zero.
    /// </summary>
    private void OnTimeOut()
    {
        AudioManager.Instance.StopLoopedEffect();
        GameManager.Instance.Lose();
    }
}
