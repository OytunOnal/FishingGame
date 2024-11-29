using UnityEngine;

/// <summary>
/// Handles the spawning of plant objects across the screen width, utilizing
/// cached screen boundaries to avoid redundant calculations.
/// </summary>
public class PlantSpawner : EnvironmentSpawner
{
    private float leftSideX;  // Left boundary of the screen in world coordinates
    private float rightSideX; // Right boundary of the screen in world coordinates
    private readonly float yPosition = -14f; // Fixed y-position for all spawned plants
    private readonly float zPosition = 0f; // Fixed z-position for all spawned plants

    private void Awake()
    {
        CacheScreenBoundaries(); // Cache screen boundaries once to avoid recalculating each spawn
    }

    /// <summary>
    /// Spawns plant objects along the x-axis between the left and right screen boundaries,
    /// with randomized spacing and unique prefab selection.
    /// </summary>
    protected override void Spawn()
    {
        float spawnPointX = leftSideX;

        // Spawn plants across the width of the screen until reaching the right boundary
        while (spawnPointX < rightSideX)
        {
            SetNewRandomObjectIndex(); // Get a unique index to avoid consecutive duplicates

            // Increment spawn position along the x-axis with random spacing
            spawnPointX += Random.Range(1f, 5f);

            // Spawn a new plant at the calculated position
            GameObject newPlant = PoolManager.Spawn("Plants_" + randomObjectIndex);
            newPlant.transform.SetParent(transform);
            newPlant.transform.localPosition = new Vector3(spawnPointX, yPosition, zPosition);
            spawnedObjects.Add(newPlant); // Track the spawned plant for future clearing
        }
    }

    /// <summary>
    /// Caches the screen boundaries by calculating the x-coordinates for the left and right
    /// edges of the screen in world space, optimizing repeated screen boundary calculations.
    /// </summary>
    private void CacheScreenBoundaries()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            rightSideX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 16)).x;
            leftSideX = mainCamera.ScreenToWorldPoint(new Vector3(0f, 0, 16)).x;
        }
        else
        {
            Debug.LogError("Main Camera not found. Screen boundaries could not be set.");
        }
    }
}
