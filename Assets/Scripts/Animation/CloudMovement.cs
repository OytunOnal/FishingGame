using UnityEngine;
using DG.Tweening;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] private Vector2 minScaleRange = new Vector2(0.8f, 1.2f); // Initial scale range
    [SerializeField] private Vector2 maxScaleRange = new Vector2(1.0f, 1.5f); // Target scale range
    [SerializeField] private float minSpeed = 1f, maxSpeed = 4f; // Speed range for movement
    [SerializeField] private float scaleDuration = 2f; // Duration of the scaling effect

    private Vector3 targetPosition;
    float speed;
    public void Initialize(Vector3 targetPos)
    {
        // Set target position for movement
        targetPosition = targetPos;
        speed = Random.Range(minSpeed, maxSpeed);

        // Set initial random scale and looped scaling effect
        float startScale = Random.Range(minScaleRange.x, minScaleRange.y);
        float endScale = Random.Range(maxScaleRange.x, maxScaleRange.y);
        transform.localScale = Vector3.one * startScale;

        // Start the scaling animation on a loop
        transform.DOScale(Vector3.one * endScale, scaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

    }

    private void Update()
    {
        // Move the cloud towards its target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Destroy the cloud when it reaches the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            PoolManager.Despawn(gameObject);
        }
    }
}
