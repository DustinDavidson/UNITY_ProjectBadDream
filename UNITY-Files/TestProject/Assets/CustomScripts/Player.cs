using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private int health;
    public int maxHealth = 100;
    public List<string> inventory = new List<string>(5);

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
        Debug.Log("Item added to inventory: " + itemName + ". Total items: " + inventory.Count + "/5");
        if(itemName == "KEY"){
            Debug.Log("Player picked up a key.");
        }
        else{
            Debug.Log("Player picked up an item: " + itemName);
        }
    }

    
}


