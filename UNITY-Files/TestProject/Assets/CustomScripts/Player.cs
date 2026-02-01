using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private int health;
    public int maxHealth = 100;
    public bool hasKey = false;
    public List<string> inventory = new List<string>(10);

    void Start()
    {
        health = maxHealth;
        Debug.Log("Player initialized with health: " + health + "/" + maxHealth);
        
    }

    void Update()
    {
       
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

    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }

    public void AddItem(string itemName)
    {
        if (inventory.Count >= 10)
        {
            Debug.Log("Inventory full. Cannot add item: " + itemName);
            return;
        }
        inventory.Add(itemName);
        Debug.Log("Item added to inventory: " + itemName);
        if(itemName == "KEY"){
            hasKey = true;
            Debug.Log("Player picked up the key.");
        }
        else{
            Debug.Log("Player picked up an item: " + itemName);
        }
    }

    
}


