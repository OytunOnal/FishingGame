using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    protected List<GameObject> spawnedObjects = new(); // Tracks currently spawned objects for easy clearing
    protected int randomObjectIndex = -1; // Index of the last spawned object to avoid duplicates

    /// <summary>
    /// Clears all spawned objects and restarts the spawning process for a new scene.
    /// </summary>
    public virtual void NewScene()
    {
        ClearObjects();
        StartSpawning();
    }

    /// <summary>
    /// Begins the spawning process; can be overridden for continuous or one-time spawning.
    /// </summary>
    public virtual void StartSpawning()
    {
        Spawn(); // Starts spawning objects
    }

    /// <summary>
    /// Despawns all currently spawned objects and clears the list.
    /// </summary>
    internal void ClearObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            PoolManager.Despawn(obj); // Return the object to the pool
        }
        spawnedObjects.Clear();
    }

    /// <summary>
    /// Abstract method to handle the actual spawning logic; implemented by derived classes.
    /// </summary>
    protected abstract void Spawn();

}
