using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelsDatabase levelsDatabase;  // Reference to the LevelsDatabase ScriptableObject
    private int currentLevelIndex = 0;  // Keeps track of the current level
    private Level currentLevel;
    // Method to open a specific level by index
    public void OpenLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        // Ensure the index is within bounds
        if (levelIndex >= 0 && levelIndex < levelsDatabase.LevelsCount)
        {
            currentLevel = levelsDatabase.GetLevel(levelIndex);
            ApplyLevelSettings();
        }
        else
        {
            Debug.LogError("Level index is out of range.");
        }
    }

    // Method to open the next level in the sequence
    public void OpenNextLevel()
    {
        int nextLevelIndex = (++currentLevelIndex) % levelsDatabase.LevelsCount;
        OpenLevel(nextLevelIndex);
    }

    // Apply the specific settings of a level (e.g., adjusting speed multiplier)
    public void ApplyLevelSettings()
    {
        ScoreManager.Instance.Initialize(currentLevel.scoreNeeded);
        UIManager.Instance.SetTimer(currentLevel.timeGiven);
        UIManager.Instance.SetEnvironment(currentLevel.environment);
    }

    // Reset the level index and start from level 0
    public void ResetLevels()
    {
        currentLevelIndex = 0;
        OpenLevel(0);
    }

    public FishSpawnSettings GetFishSpawnSettings()
    {
        return currentLevel.fishSpawnSettings;
    }

    internal int GetLevelIndex()
    {
        return currentLevelIndex;
    }
}
