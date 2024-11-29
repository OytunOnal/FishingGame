
using UnityEngine;

/// <summary>
/// Abstract base class for spawning environmental objects with unique indices,
/// such as plants, clouds, and cliffs. Handles object clearing and manages object pooling.
/// </summary>
public abstract class EnvironmentSpawner : Spawner
{
    [SerializeField] protected int objectCount = 12; // Total types of objects to spawn, e.g., 12 unique prefabs

    /// <summary>
    /// Sets a new random object index, ensuring it does not match the previous index.
    /// This helps avoid consecutive duplicate objects.
    /// </summary>
    protected void SetNewRandomObjectIndex()
    {
        int newRandomObjectIndex;
        do
        {
            newRandomObjectIndex = Random.Range(1, objectCount + 1);
        }
        while (newRandomObjectIndex == randomObjectIndex);

        randomObjectIndex = newRandomObjectIndex;
    }
}
