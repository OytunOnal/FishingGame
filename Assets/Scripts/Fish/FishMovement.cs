using UnityEngine;

public class FishMovement : MonoBehaviour
{
    // Movement settings from FishSettings
    private FishSettings.MovementSettings movementSettings;

    internal float fishSpeed = 0; // Speed of the fish
    internal int direction = 0;   // Movement direction: 1 for right, -1 for left
    private float fishAltitude = 0; // Altitude (Y-position) of the fish
    private bool isMoving = false;  // Controls whether the fish is moving

    private float floatStartTime;   // Time when the floating effect started
    private float randomizedFloatAmount; // Randomized amount for vertical movement
    private Vector3 initialRotation; // Initial rotation of the fish

    private float speedMultiplier;
    /// <summary>
    /// Initializes the movement parameters for the fish, including speed, altitude, and floating behavior.
    /// </summary>
    /// <param name="xPosition">Starting X position to set the direction</param>
    /// <param name="settings">Movement settings from FishSettings</param>
    public void Initialize(float xPosition, FishSettings.MovementSettings settings, float newSpeedMultiplier)
    {
        speedMultiplier = newSpeedMultiplier;
        movementSettings = settings;
        SetDirection(xPosition);
        SetInitialRotation();
        SetRandomizedSpeedAndAltitude(xPosition);
        SetOtherParameters();
    }

    /// <summary>
    /// Sets the movement direction based on the starting X position.
    /// </summary>
    private void SetDirection(float xPosition)
    {
        direction = xPosition > 0 ? -1 : 1;
    }

    /// <summary>
    /// Sets the initial rotation of the fish based on its movement direction.
    /// </summary>
    private void SetInitialRotation()
    {
        initialRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y * direction, 0);
    }

    /// <summary>
    /// Randomizes the fish's speed and altitude within the boundaries of the screen height.
    /// Sets the initial position of the fish based on these values.
    /// </summary>
    private void SetRandomizedSpeedAndAltitude(float xPosition)
    {
        fishSpeed = Random.Range(movementSettings.minFishSpeed, movementSettings.maxFishSpeed)* speedMultiplier;

        // Calculate the sea boundaries using the screen height in world coordinates
        float screenHeight = Screen.height;
        float topOfTheSea = Camera.main.ScreenToWorldPoint(new Vector3(0, screenHeight * 2 / 3, 16)).y;
        float bottomOfTheSea = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 16)).y;
        fishAltitude = Random.Range(bottomOfTheSea + movementSettings.bottomOffset, topOfTheSea - movementSettings.topOffset);

        // Set the fish's initial position
        transform.position = new Vector3(xPosition, fishAltitude, transform.position.z);
    }

    /// <summary>
    /// Sets additional movement parameters for the floating effect.
    /// </summary>
    private void SetOtherParameters()
    {
        // Randomize float amount for up-down movement
        randomizedFloatAmount = movementSettings.floatAmount + Random.Range(-0.2f, 0.2f);

        // Start time for the floating sine wave
        floatStartTime = Time.time;

        isMoving = true;
    }

    /// <summary>
    /// Stops the fish's movement, resetting its rotation to the initial state.
    /// </summary>
    public void StopMovement()
    {
        if (isMoving)
        {
            transform.rotation = Quaternion.Euler(initialRotation);
            isMoving = false;
        }
    }

    private void Update()
    {
        // Move the fish horizontally and vertically based on speed and direction
        if (isMoving) MoveFish();
    }

    /// <summary>
    /// Moves the fish along the X-axis according to its speed and direction.
    /// Handles vertical movement with a sine-wave float effect.
    /// </summary>
    private void MoveFish()
    {
        // Calculate new horizontal position
        float newX = transform.position.x + (fishSpeed * Time.deltaTime * direction);

        // Calculate vertical position based on a sine wave for smooth up-down movement
        float timeElapsed = (Time.time - floatStartTime) * (fishSpeed / movementSettings.maxFishSpeed);
        float newY = fishAltitude + Mathf.Sin(timeElapsed * Mathf.PI * 2) * randomizedFloatAmount;

        // Apply the new position (X and Y)
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
