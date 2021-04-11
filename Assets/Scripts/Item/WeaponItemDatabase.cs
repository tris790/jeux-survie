using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item Database", menuName = "Assets/Databases/Weapon Item Database")]
public class WeaponItemDatabase : ScriptableObject
{
    public List<WeaponItem> weaponItems = new List<WeaponItem>();
}