
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<Spawner> spawners; // List of spawners to manage

    /// <summary>
    /// Starts the spawning process for all environment spawners in the list.
    /// </summary>
    public void StartSpawning()
    {
        foreach (Spawner spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.StartSpawning();
            }
        }
    }

    /// <summary>
    /// Resets all spawners, clearing existing spawned objects and restarting the spawn process
    /// to set up for a new scene.
    /// </summary>
    public void NewScene()
    {
        foreach (Spawner spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.NewScene(); // Clears and restarts spawning for each spawner
            }
        }
    }

    public void ClearScene()
    {
        foreach (Spawner spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.ClearObjects(); // Clears and restarts spawning for each spawner
            }
        }
    }
}
