using System.Collections.Generic;
using System.Linq;

public class Database : Singleton<Database>
{
    public List<WeaponItem> weaponItems = new List<WeaponItem>();

    public WeaponItem GetWeaponById(ItemId weaponId)
    {
        return weaponItems.FirstOrDefault(w => w.itemId == weaponId);
    }
}