using UnityEngine;

/// <summary>
/// Handles the continuous spawning of cloud objects at random intervals,
/// managing direction based on the spawner’s position and initializing cloud movement.
/// </summary>
public class CloudSpawner : EnvironmentSpawner
{
    [SerializeField] private float baseSpawnInterval = 7f; // Base interval for cloud spawning
    private SpawnScheduler spawnScheduler; // Scheduler to handle spawn intervals

    private void Awake()
    {
        // Add SpawnScheduler component to handle spawning intervals
        spawnScheduler = gameObject.AddComponent<SpawnScheduler>();
    }

    /// <summary>
    /// Begins spawning clouds at randomized intervals.
    /// </summary>
    public override void StartSpawning()
    {
        StopSpawning(); // Ensure no overlapping spawns
        spawnScheduler.Configure(Spawn, Random.Range(baseSpawnInterval, baseSpawnInterval + 10));
        spawnScheduler.StartSpawning();
    }

    /// <summary>
    /// Stops the spawning process, useful for scene changes or reinitialization.
    /// </summary>
    public void StopSpawning()
    {
        spawnScheduler.StopSpawning();
    }

    /// <summary>
    /// Clears and resets the spawner for a new scene, stopping any ongoing spawning cycles.
    /// </summary>
    public override void NewScene()
    {
        StopSpawning();
        base.NewScene();
    }

    /// <summary>
    /// Spawns a single cloud at the spawner’s position, initializes its movement, and adds it to the spawn list.
    /// </summary>
    protected override void Spawn()
    {
        SetNewRandomObjectIndex(); // Get a unique index to avoid consecutive duplicates

        // Spawn a cloud from the pool with a slight randomization in y-position
        GameObject cloudInstance = PoolManager.Spawn("Cloud_" + randomObjectIndex);
        Vector3 spawnPosition = new(transform.position.x,
                                            Random.Range(transform.position.y - 1.5f, transform.position.y + 1.5f),
                                            transform.position.z);

        cloudInstance.transform.SetParent(transform);
        cloudInstance.transform.SetLocalPositionAndRotation(spawnPosition, Quaternion.identity);
        spawnedObjects.Add(cloudInstance); // Track the cloud instance for easy clearing

        // Initialize cloud movement direction based on spawner's position
        CloudMovement cloudScript = cloudInstance.GetComponent<CloudMovement>();
        if (cloudScript != null)
        {
            Vector3 targetPosition = new(-transform.position.x, transform.position.y, transform.position.z);
            cloudScript.Initialize(targetPosition);
        }
    }
}
