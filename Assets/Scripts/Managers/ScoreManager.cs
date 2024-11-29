using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private ProgressBar progressBar;

    private int currentScore;
    private int targetScore;

    // Initialize the ScoreManager with a target score and reset the current score
    public void Initialize(int targetS)
    {
        targetScore = Mathf.Max(0, targetS); // Ensure target score is non-negative
        ResetCurrentScore();
        progressBar.Initialize(targetScore);
    }

    // Update the current score by the given value
    public void UpdateScore(int value)
    {
        currentScore = Mathf.Clamp(currentScore + value, 0, targetScore); // Clamp score between 0 and target score

        // Check if the target score is reached
        if (currentScore >= targetScore)
        {
            GameManager.Instance.Win(); // Call GameOver if score reached
        }

        // Update the progress bar based on the current score
        progressBar.UpdateProgress(currentScore, targetScore);
    }

    // Reset the current score to zero
    public void ResetCurrentScore()
    {
        currentScore = 0;
        progressBar.UpdateProgress(currentScore, targetScore);
    }

    // Optionally, you can add methods to get the current score if needed elsewhere
    public int GetCurrentScore()
    {
        return currentScore;
    }
}
