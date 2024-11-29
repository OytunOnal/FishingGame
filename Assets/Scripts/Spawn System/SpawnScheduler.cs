using System;
using UnityEngine;

public class SpawnScheduler : MonoBehaviour
{
    private Action spawnAction; // Action to call for spawning
    private float spawnInterval; // Interval between spawns
    private bool isSpawning; // Control if spawning is active
    private float nextSpawnTime; // Next scheduled spawn time

    /// <summary>
    /// Configures the spawn scheduler with a spawn action and initial interval.
    /// </summary>
    /// <param name="spawnAction">Action to perform on each spawn event.</param>
    /// <param name="initialInterval">Initial interval for spawning.</param>
    public void Configure(Action spawnAction, float initialInterval)
    {
        this.spawnAction = spawnAction;
        spawnInterval = initialInterval;
        nextSpawnTime = Time.time + spawnInterval; // Schedule first spawn
    }

    /// <summary>
    /// Starts the spawn scheduler.
    /// </summary>
    public void StartSpawning()
    {
        isSpawning = true;
    }

    /// <summary>
    /// Stops the spawn scheduler.
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
    }

    private void Update()
    {
        if (isSpawning && spawnAction != null && Time.time >= nextSpawnTime)
        {
            spawnAction.Invoke(); // Call the spawn action
            nextSpawnTime = Time.time + spawnInterval; // Schedule next spawn
        }
    }

    /// <summary>
    /// Sets a new spawn interval.
    /// </summary>
    /// <param name="interval">New interval for spawning.</param>
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
        nextSpawnTime = Time.time + spawnInterval; // Reschedule next spawn
    }
}
