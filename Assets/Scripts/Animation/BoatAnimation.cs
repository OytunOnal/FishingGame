using UnityEngine;
using DG.Tweening;

public class BoatAnimation : MonoBehaviour
{
    [SerializeField] private float moveAmount = 0.5f;  // Amount of vertical movement
    [SerializeField] private float moveDuration = 2f;  // Duration of one up-down float cycle
    [SerializeField] private float rotationAmount = 5f; // Rotation angle in degrees
    [SerializeField] private float rotationDuration = 1.5f; // Duration of one left-right rotation cycle

    private void Start()
    {
        // Floating up and down animation
        transform.DOMoveY(transform.position.y + moveAmount, moveDuration)
            .SetEase(Ease.InOutSine) // Smooth in-out easing
            .SetLoops(-1, LoopType.Yoyo); // Infinite yoyo loop for up and down

        // Left-right rotation animation
        transform.DORotate(new Vector3(0f, 0f, rotationAmount), rotationDuration)
            .SetEase(Ease.InOutSine) // Smooth in-out easing
            .SetLoops(-1, LoopType.Yoyo) // Infinite yoyo loop for rotation
            .SetRelative(); // Relative to current rotation, not world axis
    }
}
