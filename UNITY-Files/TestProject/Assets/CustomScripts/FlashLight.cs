using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public bool isOn = false; // flashlight state
    public float batteryLife = 100f; // battery life percentages
    public float drainRate = 3f; // battery drain per second
    public float rechargeRate = 5f; // battery recharge per second
    public bool heldByPlayer = false; // is the flashlight held by the player
    public float startDim = 40f; // When the flashlight starts dimming
    public float maxBrightness = 1f;
    private Light flashlight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = false;
        flashlight.intensity = maxBrightness;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!heldByPlayer)
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        // Drain battery if flashlight is on
        if (isOn)
        {
            DrainBattery();
        }
        // Recharge battery while holding the R key
        if (Input.GetKey(KeyCode.R) && heldByPlayer)
        {
            RechargeBattery();
        }
    }
    

    // Toggle the flashlight on or off
    public void ToggleFlashlight()
    {
       if(batteryLife > 0 && !isOn)
       {
           isOn = true;
           flashlight.enabled = true;
           Debug.Log("Flashlight turned ON");
       }
       else if(isOn)
       {
           isOn = false;
           flashlight.enabled = false;
           Debug.Log("Flashlight turned OFF");
       }
       else
       {
            flashlight.enabled = false;
           Debug.Log("Battery dead. Cannot turn on flashlight.");
       }
    }

    // Drain battery life over time
    public void DrainBattery()
    {
        if (!isOn)
            return;

        batteryLife -= drainRate * Time.deltaTime;
        batteryLife = Mathf.Clamp(batteryLife, 0f, 100f);

        UpdateIntensity();

        if (batteryLife <= 0f)
        {
            isOn = false;
            flashlight.enabled = false;
        }
    }


    // Recharge battery life over time
    public void RechargeBattery()
    {
        batteryLife += rechargeRate * Time.deltaTime;
        batteryLife = Mathf.Clamp(batteryLife, 0f, 100f);

        UpdateIntensity();
    }



    // Function to change the intensity of the flashlight
    void UpdateIntensity()
        {
            float brightness = batteryLife / startDim;
            brightness = Mathf.Clamp(brightness, 0f, 1f);
            flashlight.intensity = brightness * maxBrightness;
        }

    

    public void OnPickedUp()
    {
        heldByPlayer = true;
        Debug.Log("✅ OnPickedUp() called - heldByPlayer is now TRUE");
    }

    public void OnDropped()
    {
        heldByPlayer = false;
        Debug.Log("Flashlight dropped by player.");
    }

}
