using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private bool _isInputEnabled = true; // Track whether input is enabled

    private void Update()
    {
        if (_isInputEnabled) // Check if input is enabled
        {
            HandleMouseInput(); // Handle mouse input
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HandleFishClick(hit.collider); // Handle clicking on a fish
            }
        }
    }

    private void HandleFishClick(Collider collider)
    {
        Fish clickedFish = collider.GetComponent<Fish>();
        if (clickedFish != null)
        {
            clickedFish.OnFishClicked(); // Call the click method on the fish
        }
    }

    // Method to enable input
    public void EnableInput()
    {
        _isInputEnabled = true;
    }

    // Method to disable input
    public void DisableInput()
    {
        _isInputEnabled = false;
    }
}
