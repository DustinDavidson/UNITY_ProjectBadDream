using UnityEngine;
using UnityEngine.UI;

public class GamePlayHUD : MonoBehaviour
{

    public Image healthFill;
    public Image batteryFill;
    public Player player;
    public FlashLight flashlight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        batteryFill.gameObject.SetActive(flashlight.heldByPlayer);
        healthFill.fillAmount = player.health / 100f;

        if (flashlight.heldByPlayer)
        {
            batteryFill.fillAmount = flashlight.batteryLife / 100f;
        }
        
        
    }
}
