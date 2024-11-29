using UnityEngine;
using UnityEngine.UI;

public class FishHealth : MonoBehaviour
{
    [SerializeField] private Image healthFillerImage; // UI Image to show health progress
    private float decreaseAmount = 0; // Amount to decrease health per click
    internal bool isDead = false; // Indicates if the fish is "dead"
    private Timer timer; // Timer instance to reset health

    /// <summary>
    /// Initializes health based on the number of clicks required to "kill" the fish.
    /// </summary>
    /// <param name="requiredClicks">Number of clicks needed to fill the health bar</param>
    public void Initialize(int requiredClicks)
    {
        decreaseAmount = 1.0f / requiredClicks;
        Reset();

        // Initialize the timer to reset health after a brief duration
        timer = new(2 , Reset);

        // Set fish rotation based on initial position
        SetInitialRotation();
    }

    /// <summary>
    /// Decreases health and checks if the fish is dead.
    /// </summary>
    internal void Decrease()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            timer.Restart(); // Start timer when health UI is activated
        }
        else 
        {
            timer.Restart(); // Restart the timer if already active
        }

        // Increase fill amount based on decrease amount
        healthFillerImage.fillAmount += decreaseAmount;

        // Check if health is full, marking fish as dead
        if (healthFillerImage.fillAmount >= 1)
        {
            isDead = true;
            timer.StopTimer();
        }
    }

    /// <summary>
    /// Resets health and visibility of the health UI.
    /// </summary>
    private void Reset()
    {
        isDead = false;
        healthFillerImage.fillAmount = 0;
        gameObject.SetActive(false); // Hide health UI
    }

    /// <summary>
    /// Sets the fish rotation based on its position.
    /// </summary>
    private void SetInitialRotation()
    {
        var rotationY = transform.position.x > 0 ? 90 : 270;
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}
