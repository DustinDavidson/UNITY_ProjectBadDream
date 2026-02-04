using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = true;
    public bool isOpen = false;
    public string requiredKeyName = "Keys";  // Name of the key item needed

    void Start()
    {
        // Initialize door state
    }


        // Method to toggle lock state based on player's inventory
        public void LockToggle(Player player)
        {
        if (player == null)
        {
            Debug.Log("Player reference is null.");
            return;
        }
        if (player.HasItem(requiredKeyName) && isLocked)
        {
            isLocked = false;
            Debug.Log("Door unlocked!");
        }
        else if (!player.HasItem(requiredKeyName))
        {
            Debug.Log("Player does not have the key.");
        }
        else
        {
            Debug.Log("Door is already unlocked.");
        }
    }

    public void ToggleDoor()
    {
        if (!isLocked)
        {
            isOpen = !isOpen;
            Debug.Log("Door is now " + (isOpen ? "open." : "closed."));
        }
        else
        {
            Debug.Log("Door is locked. Cannot open.");
        }
    }
}
