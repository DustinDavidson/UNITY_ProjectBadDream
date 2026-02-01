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

    public void TryOpenDoor(Player player, FlashLight flashlight)
    {
        if (player.hasKey && flashlight.isOn)
        {
            isOpen = true;
            Debug.Log("Door opened!");
        }
        else if (!player.hasKey)
        {
            Debug.Log("Player does not have the key.");
        }
        else if (!flashlight.isOn)
        {
            Debug.Log("Flashlight is off.");
        }
        else
        {
            Debug.Log("Cannot open door. Key missing and flashlight is off.");
        }
    }
}
