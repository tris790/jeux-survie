using UnityEngine;

public enum AmmoType { Pistol, Ak };

[CreateAssetMenu(fileName = "New Ammo Item", menuName = "Assets/AmmoItem")]
public class AmmoItem : Item
{
    public AmmoType ammoType;

    public override void Use()
    {
        int ammount = Random.Range(25, 225);
        GameManager.Instance.player.GetComponent<Inventory>().AddItem(ammoType, ammount);
    }
}