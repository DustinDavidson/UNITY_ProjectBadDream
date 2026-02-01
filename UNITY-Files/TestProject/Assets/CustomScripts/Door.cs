using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;

    void Start()
    {
        // Initialize door state
    }

    void Update()
    {
        // Update logic here
    }

    public void TryOpenDoor(Player player)
    {
        if (player.hasKey)
        {
            isOpen = true;
            Debug.Log("Door opened!");
        }
        else if (!player.hasKey)
        {
            Debug.Log("Player does not have the key.");
        }
        else
        {
            Debug.Log("Player reference is null.");
        }
    }
}
