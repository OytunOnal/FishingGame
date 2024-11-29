using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelSystem/Level")]
public class Level : ScriptableObject
{
    public FishSpawnSettings fishSpawnSettings = new FishSpawnSettings();

    public int scoreNeeded;
    public int timeGiven;
    public string environment;
}
