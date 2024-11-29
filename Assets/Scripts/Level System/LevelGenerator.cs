using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LevelGenerator : MonoBehaviour
{
    private int numberOfLevels = 30;
    private int finalScoreNeeded = 500;
    private int initialScoreNeeded = 100;

    private float finalTimeGiven = 75f;
    private float initialTimeGiven = 90f;

    private float finalSmallFishWeight = 0.3f;
    private float initialSmallFishWeight = 0.7f;

    private float bonusFishProbabilityIncrease = 0.005f;
    private float initialSpeedMultiplier = 1f;
    private float speedMultiplierIncrease = 0.03f;

    private float initialSpawnInterval = 5f;
    private float spawnIntervalReduction = 0.02f;
    private float spawnIntervalRandomRange = 2.5f;

    private float minSpawnInterval = 0.6f;
    private float maxSpeedMultiplier = 2.0f;
    private float maxBonusFishProbability = 15f;

    private List<string> smallFishPool = new() { "SmallFish_1", "SmallFish_2", "SmallFish_3", "SmallFish_4", "SmallFish_5", "SmallFish_6" };
    private List<string> bigFishPool = new() { "BigFish_1", "BigFish_2", "BigFish_3", "BigFish_4", "BigFish_5", "BigFish_6" };

    private void Start()
    {
        GenerateLevels();
    }

    private void GenerateLevels()
    {
        StringBuilder levelData = new StringBuilder();

        for (int i = 1; i <= numberOfLevels; i++)
        {
            var level = ScriptableObject.CreateInstance<Level>();

            float progress = (float)(i - 1) / (numberOfLevels - 1);

            level.scoreNeeded = Mathf.RoundToInt(Mathf.Lerp(initialScoreNeeded, finalScoreNeeded, progress));
            level.timeGiven = (int)Mathf.Lerp(initialTimeGiven, finalTimeGiven, progress);

            level.fishSpawnSettings = new FishSpawnSettings
            {
                smallFishWeight = Mathf.Lerp(initialSmallFishWeight, finalSmallFishWeight, progress),
                bigFishWeight = 1f - Mathf.Lerp(initialSmallFishWeight, finalSmallFishWeight, progress),
                bonusFishProbability = Mathf.Min(maxBonusFishProbability, bonusFishProbabilityIncrease * i),
                speedMultiplier = Mathf.Min(maxSpeedMultiplier, initialSpeedMultiplier + (speedMultiplierIncrease * (i - 1))),
                fishSpawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval - (spawnIntervalReduction * (i - 1))),
                fishSpawnIntervalRandomRange = spawnIntervalRandomRange,
                smallFishPool = new List<string>(smallFishPool),
                bigFishPool = new List<string>(bigFishPool),
            };

            levelData.AppendLine($"Level {i}:");
            levelData.AppendLine($"Score Needed: {level.scoreNeeded}");
            levelData.AppendLine($"Time Given: {level.timeGiven} seconds");
            levelData.AppendLine($"Small Fish Weight: {level.fishSpawnSettings.smallFishWeight}");
            levelData.AppendLine($"Big Fish Weight: {level.fishSpawnSettings.bigFishWeight}");
            levelData.AppendLine($"Bonus Fish Probability: {level.fishSpawnSettings.bonusFishProbability}%");
            levelData.AppendLine($"Speed Multiplier: {level.fishSpawnSettings.speedMultiplier}");
            levelData.AppendLine($"Fish Spawn Interval: {level.fishSpawnSettings.fishSpawnInterval} seconds");
            levelData.AppendLine(new string('-', 40));

            SaveLevel(level, i);
        }

        WriteToFile(levelData.ToString());
    }

    private void WriteToFile(string levelData)
    {
        string path = Path.Combine(Application.dataPath, "Games", "PurrfectCatch", "Levels", "LevelData.txt");

        if (!Directory.Exists(Path.Combine(Application.dataPath, "Games", "PurrfectCatch", "Levels")))
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Games", "PurrfectCatch", "Levels"));
        }

        File.WriteAllText(path, levelData);
        Debug.Log($"Level data saved to {path}");
    }

    private void SaveLevel(Level level, int levelIndex)
    {
#if UNITY_EDITOR
        string assetPath = $"Assets/Games/PurrfectCatch/Levels/Level_{levelIndex}.asset";
        UnityEditor.AssetDatabase.CreateAsset(level, assetPath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"Level {levelIndex} generated and saved to {assetPath}");
#endif
    }
}
