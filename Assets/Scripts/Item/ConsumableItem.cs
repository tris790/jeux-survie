using UnityEngine;

public enum ConsumableType { Health, Stamina };

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Assets/ConsumableItem")]
public class ConsumableItem : Item
{
    public ConsumableType consumeType;

    public override void Use()
    {
        if (consumeType == ConsumableType.Health)
        {
            int value = Random.Range(10, 20);
            GameManager.Instance.player.GetComponent<HealthComponent>().AddOrRemove(value);
        }
    }
}