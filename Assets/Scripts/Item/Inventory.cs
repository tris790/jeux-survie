using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{
    public int AkRound;
    public GameObject akRoundNotifier;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUi();
    }

    public int GetAmmoCount(AmmoType ammoType)
    {
        if (ammoType == AmmoType.Ak)
        {
            return AkRound;
        }
        else if (ammoType == AmmoType.Pistol)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void AddItem(AmmoType ammoType, int amount)
    {
        if (ammoType == AmmoType.Ak)
        {
            AkRound += amount;
        }

        UpdateUi();
    }

    public void Remove(AmmoType ammoType, int amount)
    {
        if (ammoType == AmmoType.Ak)
        {
            GameManager.Instance.player.GetComponent<Inventory>().AkRound -= amount;
        }

        UpdateUi();
    }

    void UpdateUi()
    {
        akRoundNotifier.GetComponent<Text>().text = AkRound.ToString();
    }
}