using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class Timer
{
    private float startTime; // Stores the initial start time of the timer
    private float currentTime; // Stores the remaining time in seconds
    private bool isTimerRunning; // Flag to control if the timer is running
    private readonly Action onTimerEndCallback; // Callback function when the timer reaches zero
    private CancellationTokenSource cancellationTokenSource; // To handle task cancellation

    /// <summary>
    /// Constructor for the Timer class, initializing the timer with a start time and an end callback.
    /// </summary>
    /// <param name="startTime">Initial duration of the timer in seconds.</param>
    /// <param name="onTimerEndCallback">Action to invoke when the timer reaches zero.</param>
    public Timer(float startTime, Action onTimerEndCallback)
    {
        currentTime = this.startTime = startTime;
        this.onTimerEndCallback = onTimerEndCallback;
        isTimerRunning = false;
    }

    /// <summary>
    /// Starts the timer if it is not already running.
    /// </summary>
    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
            cancellationTokenSource = new CancellationTokenSource(); // Create a new cancellation token
            RunTimerAsync(cancellationTokenSource.Token).Forget(); // Start the async timer task
        }
    }

    /// <summary>
    /// Stops the timer and cancels the running task.
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;
        cancellationTokenSource?.Cancel(); // Cancel the task if it’s running
        cancellationTokenSource = null;
    }

    /// <summary>
    /// Restarts the timer from the initial start time.
    /// </summary>
    public void Restart()
    {
        StopTimer(); // Stop any existing task
        currentTime = startTime;
        StartTimer();
    }

    /// <summary>
    /// Asynchronous timer loop that counts down based on `Time.deltaTime`.
    /// </summary>
    private async UniTask RunTimerAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (isTimerRunning && currentTime > 0)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken); // Wait for the next frame, check for cancellation

                currentTime -= Time.deltaTime; // Decrease remaining time by frame duration

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    isTimerRunning = false;
                    onTimerEndCallback?.Invoke(); // Invoke callback when timer reaches zero
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Handle task cancellation gracefully if needed
        }
    }

    /// <summary>
    /// Sets the timer to a new start time and stops it if it's currently running.
    /// </summary>
    /// <param name="newStartTimeInSeconds">New duration for the timer in seconds.</param>
    public void SetTimer(float newStartTimeInSeconds)
    {
        currentTime = startTime = newStartTimeInSeconds;
        isTimerRunning = false;
    }

    /// <summary>
    /// Adds additional time to the current timer duration.
    /// </summary>
    /// <param name="secondsToAdd">Time to add in seconds.</param>
    public void AddTime(float secondsToAdd)
    {
        currentTime += secondsToAdd;
    }

    /// <summary>
    /// Retrieves the current time remaining in minutes and seconds format.
    /// </summary>
    /// <returns>A tuple of minutes and seconds remaining on the timer.</returns>
    public (int minutes, int seconds) GetTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        return (minutes, seconds);
    }

    /// <summary>
    /// Checks if the timer is currently active and running.
    /// </summary>
    /// <returns>True if the timer is running, otherwise false.</returns>
    public bool IsTimerRunning()
    {
        return isTimerRunning;
    }
}
