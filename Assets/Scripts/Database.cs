using System.Collections.Generic;
using System.Linq;

public class Database : Singleton<Database>
{
    public List<WeaponItem> weaponItems = new List<WeaponItem>();
    public List<AmmoItem> ammoItems = new List<AmmoItem>();

    public WeaponItem GetWeaponById(ItemId weaponId)
    {
        return weaponItems.FirstOrDefault(w => w.itemId == weaponId);
    }

    public AmmoItem GetAmmoItems(ItemId ammoId)
    {
        return ammoItems.FirstOrDefault(w => w.itemId == ammoId);
    }
}