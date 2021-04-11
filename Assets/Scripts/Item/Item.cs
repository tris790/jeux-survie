using UnityEngine;

public enum ItemType { Consumable, Weapon };

public enum ItemId
{
    Pistol,
    Ak,
    TroussePS
}

public abstract class Item : ScriptableObject
{
    public ItemId itemId;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemIcon;
    public ItemType itemType;

    public virtual void Use()
    {
    }
}