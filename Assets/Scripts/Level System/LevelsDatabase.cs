using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelsDatabase", menuName = "LevelSystem/LevelDatabase")]
public class LevelsDatabase : ScriptableObject
{
    [SerializeField] private Level[] levels;

    public int LevelsCount
    {
        get { return levels.Length; }
    }

    public Level GetLevel(int index)
    {
        return levels[index % levels.Length];
    }
}
