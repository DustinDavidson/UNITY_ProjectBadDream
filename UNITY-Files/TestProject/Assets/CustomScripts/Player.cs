using UnityEngine;

public class Player : MonoBehaviour
{
    private int health;
    public int maxHealth = 100;
    public bool hasKey = false;

    void Start()
    {
        health = maxHealth;
        Debug.Log("Player initialized with health: " + health + "/" + maxHealth);
        
    }

    void Update()
    {
        // Pickup key
        if (Input.GetKeyDown(KeyCode.E))
        {
            hasKey = true;
            Debug.Log("Picked up key!");
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Healed. Health = " + health + "/" + maxHealth);
    }

    public void TakeDamage(int amount){
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Took damage. Health = " + health + "/" + maxHealth);

    }

    
}


