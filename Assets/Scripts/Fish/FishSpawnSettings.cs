using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishSpawnSettings
{
    [Range(0f, 1f)]
    public float smallFishWeight = 0.7f;

    [Range(0f, 1f)]
    public float bigFishWeight = 0.3f; 

    [Range(0, 100)]
    public float bonusFishProbability = 0.1f;
    public float speedMultiplier = 1f;
    public float fishSpawnInterval = 2f;
    public float fishSpawnIntervalRandomRange = 2f;

    public List<string> smallFishPool = new List<string>
    {
        "SmallFish_1", "SmallFish_2", "SmallFish_3",
        "SmallFish_4", "SmallFish_5", "SmallFish_6"
    };

    public List<string> bigFishPool = new List<string>
    {
        "BigFish_1", "BigFish_2", "BigFish_3",
        "BigFish_4", "BigFish_5", "BigFish_6"
    };

    internal readonly List<string> bonusFishPool = new() { "BonusFish" };
}
