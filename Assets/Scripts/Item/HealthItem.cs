using UnityEngine;

[CreateAssetMenu(fileName = "New Health Item", menuName = "Assets/HealthItem")]
public class HealthItem : PickableItem
{
    // Start is called before the first frame update
    public int health;
}