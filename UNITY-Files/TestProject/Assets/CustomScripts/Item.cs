using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // Identifyers
    public string itemName; // The name of the item
    public string itemID; // The identifyer of the item ("KEY01")
    public string description; // A short description of the item
    public bool isConsumable; // Whether or not the item is consumable
    

    // Visuals
    public Sprite icon; // Image for UI
    public GameObject prefab; // World Object

    // Quantity
    public int maxStack; // Max ammount for an item in inventory

    

    // Type
    public enum ItemType {FLASHLIGHT, KEY, OTHER}
    public ItemType itemType;

    // ONLY FOR KEYS
    public string doorID;
}