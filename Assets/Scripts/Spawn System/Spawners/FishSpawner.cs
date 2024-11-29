using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : Spawner
{
    [SerializeField] private float baseSpawnInterval = 2f; // Base interval for fish spawning
    [SerializeField] private bool isLeftSpawner; // Determines if spawner is on the left side of the screen

    private FishSpawnSettings fishSpawnSettings;
    private SpawnScheduler spawnScheduler;

    private void Awake()
    {
        // Add a SpawnScheduler component to this spawner
        spawnScheduler = gameObject.AddComponent<SpawnScheduler>();
    }

    private void Start()
    {
        PositionSpawner();
    }

    /// <summary>
    /// Initializes the spawner with fish spawn settings.
    /// </summary>
    public void Initialize(FishSpawnSettings newFishSpawnSettings)
    {
        fishSpawnSettings = newFishSpawnSettings;
        spawnScheduler.Configure(Spawn, Random.Range(newFishSpawnSettings.fishSpawnInterval, newFishSpawnSettings.fishSpawnIntervalRandomRange));
    }

    /// <summary>
    /// Dynamically positions the spawner based on screen boundaries.
    /// </summary>
    private void PositionSpawner()
    {
        float positionX = isLeftSpawner ?
            Camera.main.ScreenToWorldPoint(new Vector3(0f, 0, 16)).x :
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 16)).x;

        transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Begins spawning fish at configured intervals.
    /// </summary>
    public override void StartSpawning()
    {
        Initialize(LevelManager.Instance.GetFishSpawnSettings());
        spawnScheduler.StartSpawning(); // Start the spawn scheduler
    }

    /// <summary>
    /// Stops the spawning process.
    /// </summary>
    public void StopSpawning()
    {
        spawnScheduler.StopSpawning();
    }

    /// <summary>
    /// Resets the spawner for a new scene, stopping ongoing spawning cycles.
    /// </summary>
    public override void NewScene()
    {
        StopSpawning();
        base.NewScene();
    }

    /// <summary>
    /// Spawns a fish based on weight probabilities, including a chance to spawn bonus fish.
    /// </summary>
    protected override void Spawn()
    {
        string fishType = Random.value < fishSpawnSettings.smallFishWeight ?
            GetRandomFish(fishSpawnSettings.smallFishPool) :
            GetRandomFish(fishSpawnSettings.bigFishPool);

        // Spawn the chosen fish
        SpawnFish(fishType);

        // Spawn bonus fish with a small probability
        if (Random.Range(0,100) < fishSpawnSettings.bonusFishProbability)
        {
            SpawnFish(GetRandomFish(fishSpawnSettings.bonusFishPool));
        }
    }

    /// <summary>
    /// Returns a random fish type from a specified pool.
    /// </summary>
    private string GetRandomFish(List<string> fishPool)
    {
        return fishPool[Random.Range(0, fishPool.Count)];
    }

    /// <summary>
    /// Spawns a fish of the specified type and initializes it.
    /// </summary>
    private void SpawnFish(string fishType)
    {
        Fish newFish = PoolManager.Spawn(fishType)?.GetComponent<Fish>();
        if (newFish != null)
        {
            newFish.InitFish(transform.position.x, fishSpawnSettings.speedMultiplier);
        }
        else
        {
            Debug.LogWarning($"Failed to spawn {fishType}. Ensure it's set up correctly in the pool.");
        }
    }
}
