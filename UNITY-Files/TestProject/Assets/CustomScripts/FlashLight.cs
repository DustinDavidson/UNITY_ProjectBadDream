using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light flashlight; // reference to the Light component
    public bool isOn = false; // flashlight state
    public float batteryLife = 100f; // battery life percentages
    public float drainRate = 0.2f; // battery drain per second
    public float rechargeRate = 0.25f; // battery recharge per second

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
    {
            // Drain battery if flashlight is on
        if (isOn)
        {
            DrainBattery();
        }
        // Recharge battery while holding the R key
        if (Input.GetKey(KeyCode.R))
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
        if(isOn && batteryLife > 0)
        {
            batteryLife -= drainRate;
            batteryLife = Mathf.Clamp(batteryLife, 0, 100);
            Debug.Log("Battery draining. Current battery life: " + batteryLife + "%");
            if(batteryLife == 0)
            {
                isOn = false;
                flashlight.enabled = false;
                Debug.Log("Battery dead. Flashlight turned OFF");
            }
        }
    }

    // Recharge battery life over time
    public void RechargeBattery()
    {
        batteryLife += rechargeRate;
        batteryLife = Mathf.Clamp(batteryLife, 0, 100);
        Debug.Log("Battery recharging. Current battery life: " + batteryLife + "%");
        if(batteryLife == 100)
        {
            Debug.Log("Battery fully charged.");
        }
    }

}
