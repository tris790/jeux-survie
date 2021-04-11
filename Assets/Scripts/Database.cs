using System.Collections.Generic;
using System.Linq;

public class Database : Singleton<Database>
{
    public List<WeaponItem> weaponItems = new List<WeaponItem>();
    public List<AmmoItem> ammoItems = new List<AmmoItem>();
    public List<ConsumableItem> consumableItems = new List<ConsumableItem>();

    public WeaponItem GetWeaponItem(ItemId weaponId)
    {
        return weaponItems.FirstOrDefault(w => w.itemId == weaponId);
    }

    public AmmoItem GetAmmoItem(ItemId ammoId)
    {
        return ammoItems.FirstOrDefault(w => w.itemId == ammoId);
    }

    public ConsumableItem GetConsumableItem(ItemId consumableId)
    {
        return consumableItems.FirstOrDefault(w => w.itemId == consumableId);
    }
}