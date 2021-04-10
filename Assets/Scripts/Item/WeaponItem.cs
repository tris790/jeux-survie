
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Assets/WeaponItem")]
public class WeaponItem : Item
{
    public float damage;
    public float fireRate;
    public float bulletForce;
    public string bulletTag;
    public AmmoType bulletType;
}