using UnityEngine;
using System.Collections.Generic; 

public class Player : MonoBehaviour
{
    private int health;
    public int maxHealth = 100;
    public List<ItemInstance> inventory = new List<ItemInstance>(5);
    public Dictionary<string, ItemInstance> inventoryLookup = new Dictionary<string, ItemInstance>();

    void Start()
    {
        health = maxHealth;
        Debug.Log("Player initialized with health: " + health + "/" + maxHealth);
        
    }

    void Update()
    {
       
    }

    // Function to let the player heal their health
    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Healed. Health = " + health + "/" + maxHealth);
    }

    // Function to have the player take damage
    public void TakeDamage(int amount){
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Took damage. Health = " + health + "/" + maxHealth);

    }

    // Function to check inventory for specifyed item
    public bool HasItem(string itemName)
    {
        return inventoryLookup.ContainsKey(itemName);
    }

    // Function to add specifyed item to inventory
    public void AddItem(Item item)
    {
        if (inventory.Count >= 5)
        {
            Debug.Log("Inventory full. Cannot add item: " + item.itemName);
            return;
        }

        ItemInstance currentItem = new ItemInstance(item);
        inventory.Add(currentItem);
        inventoryLookup.Add(currentItem.data.itemName, currentItem);
        Debug.Log("Item added to inventory: " + currentItem.data.itemName + ". Total items: " + inventory.Count + "/5");

        if(currentItem.data.itemType == Item.ItemType.KEY){
            Debug.Log("Player picked up a key.");
        }
        else{
            Debug.Log("Player picked up an item: " + currentItem.data.itemName);
        }
    }

    
}


