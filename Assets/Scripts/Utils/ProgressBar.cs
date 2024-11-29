using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI targetScoreText;

    // Initialize the progress bar with the target score
    public void Initialize(int targetScore)
    {
        currentScoreText.text = "0";
        targetScoreText.text = targetScore.ToString();
        progressBar.fillAmount = 0f; // Set progress to 0
    }

    // Update the progress based on the current score
    public void UpdateProgress(int currentScore, int targetScore)
    {
        currentScoreText.text = currentScore.ToString();

        // Update the progress bar fill
        progressBar.fillAmount = (float)currentScore / targetScore; 
    }
}
