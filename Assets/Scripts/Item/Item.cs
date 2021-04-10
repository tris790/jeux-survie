using UnityEngine;

public enum ItemType { Consumable, Weapon };

public abstract class Item : ScriptableObject
{
    public string itemID;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemIcon;
    public ItemType itemType;

    public virtual void Use()
    {
    }
}