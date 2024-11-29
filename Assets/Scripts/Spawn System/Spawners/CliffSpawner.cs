using UnityEngine;

/// <summary>
/// Handles the spawning of cliff objects across the screen width when activated,
/// with configurable spacing and random y-axis positioning.
/// </summary>
public class CliffSpawner : EnvironmentSpawner
{
    private float screenWidth; // Width of the screen in world units

    [SerializeField] private bool isSpawning = false; // Toggle for initiating spawning

    private void Start()
    {
        CacheScreenWidth(); // Cache screen width once on startup
    }

    private void Update()
    {
        // Trigger spawning only when the isSpawning flag is set
        if (isSpawning)
        {
            ClearObjects(); // Clear previous cliffs before spawning new ones
            Spawn(); // Spawn new cliffs across the screen width
            isSpawning = false; // Reset the spawning flag after one cycle
        }
    }

    /// <summary>
    /// Spawns cliff objects along the x-axis across the screen width, with randomized
    /// spacing and y-axis positioning to simulate natural placement.
    /// </summary>
    protected override void Spawn()
    {
        float spawnPointX = screenWidth / 8; // Start at the left edge of the screen

        // Spawn cliffs at intervals until reaching the screen width
        while (spawnPointX < screenWidth)
        {
            SetNewRandomObjectIndex(); // Get a unique cliff prefab index

            // Spawn and position the cliff
            GameObject newCliff = PoolManager.Spawn(UIManager.Instance.EnvironmentName +"_Cliff_" + randomObjectIndex);
            newCliff.transform.SetParent(transform);
            newCliff.transform.localScale = Vector3.one * 1.5f; // Scale up the cliff size
            newCliff.transform.localPosition = new Vector3(
                spawnPointX,
                0,
                -77 // Fixed z-position for all cliffs
            );
            spawnPointX += Random.Range(3 * screenWidth / 8, 4 * screenWidth / 8); // Randomized x-axis spacing

            spawnedObjects.Add(newCliff); // Track spawned cliffs for easy clearing
        }
    }

    /// <summary>
    /// Caches the screen width using the RectTransform of the parent,
    /// avoiding repeated calculations during spawning.
    /// </summary>
    private void CacheScreenWidth()
    {
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        if (parentRect != null)
        {
            screenWidth = parentRect.rect.width;
        }
        else
        {
            Debug.LogError("Parent RectTransform not found. Screen width could not be set.");
        }
    }
}
