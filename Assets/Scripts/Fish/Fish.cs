using UnityEngine;

public class Fish : MonoBehaviour
{
    // Serialized FishSettings to hold customizable properties for each fish type.
    [SerializeField] protected FishSettings fishSettings;

    // Private references to various components and settings
    private GameObject fishModel;
    private FishHealth fishHealth;
    private FishAnimator fishAnimator;
    private FishMovement fishMovement;


    private void Awake()
    {
        // Initialize essential components and validate settings
        InitializeComponents();
        InitializeSettings();
    }

    /// <summary>
    /// Initializes all necessary component references.
    /// </summary>
    private void InitializeComponents()
    {
        fishModel = transform.GetChild(0).gameObject;
        fishMovement = gameObject.AddComponent<FishMovement>(); 
        fishHealth = GetComponentInChildren<FishHealth>();
        fishAnimator = gameObject.AddComponent<FishAnimator>();
    }

    /// <summary>
    /// Ensures that the FishSettings have been assigned. 
    /// Logs an error if FishSettings are missing.
    /// </summary>
    private void InitializeSettings()
    {
        if (fishSettings == null)
        {
            Debug.LogError($"FishSettings not assigned for {gameObject.name}");
            return;
        }
    }

    /// <summary>
    /// Handles collision with objects, primarily for out-of-screen detection.
    /// Despawns the fish if it collides with an "OutOfScreen" trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OutOfScreen"))
        {
            Despawn();
        }
    }

    /// <summary>
    /// Initializes the fish's movement, health, and animator speed with given parameters.
    /// </summary>
    /// <param name="xPosition">X position where the fish spawns</param>
    /// <param name="newSpeedMultiplier">Speed multiplier to adjust fish speed</param>
    public void InitFish(float xPosition, float newSpeedMultiplier)
    {
        fishModel.SetActive(true);

        // Initialize movement, health, and animation based on settings
        fishMovement.Initialize(xPosition,fishSettings.movementSettings, newSpeedMultiplier);
        fishHealth.Initialize(fishSettings.requiredClicks);
        fishAnimator.SetSpeed(0.25f + fishMovement.fishSpeed / 10);
    }

    /// <summary>
    /// Handles the fish click event. Plays animations and decreases fish health.
    /// Spawns a particle effect for feedback.
    /// </summary>
    public void OnFishClicked()
    {
        fishAnimator.ShakeAndFade();
        fishHealth.Decrease();
        PoolManager.Spawn(fishSettings.clickParticle).transform.position = transform.position;
        AudioManager.Instance.PlayEffect(fishSettings.clickSound);

        // Check if the fish has reached zero health, and handle death if true
        if (fishHealth.isDead)
        {
            OnFishDied();
        }
    }

    /// <summary>
    /// Called when the fish health reaches zero. Plays death particle, triggers 
    /// animations and updates the score, then despawns the fish.
    /// </summary>
    private void OnFishDied()
    {
        SpawnScoreText(fishSettings.points);
        PoolManager.Spawn(fishSettings.catchParticle).transform.position = transform.position;
        AudioManager.Instance.PlayEffect(fishSettings.catchSound);
        CatAnimation.Instance.CatchFish();

        ScoreManager.Instance.UpdateScore(fishSettings.points);
        Despawn();
    }

    public void SpawnScoreText(int score)
    {
        GameObject scoreTextObject = PoolManager.Spawn("ScoreTextEffect");
        scoreTextObject.transform.position = transform.position;
        ScoreTextEffect scoreTextEffect = scoreTextObject.GetComponent<ScoreTextEffect>();
        scoreTextEffect.PlayEffect(score);
    }

    /// <summary>
    /// Despawns the fish by stopping all animations and movement, hiding the model, 
    /// and returning it to the object pool.
    /// </summary>
    public void Despawn()
    {
        fishAnimator.StopShakeAndFade();
        fishMovement.StopMovement();
        fishModel.SetActive(false);
        PoolManager.Despawn(gameObject);
    }
}
