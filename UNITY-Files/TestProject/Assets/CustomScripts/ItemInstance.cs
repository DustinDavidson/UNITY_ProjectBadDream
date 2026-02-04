using UnityEngine;

public class ItemInstance
{
    public Item data;
    public int quantity;

    public ItemInstance(Item data, int quantity = 1)
    {
        this.data = data;
        this.quantity = quantity;
    }
}