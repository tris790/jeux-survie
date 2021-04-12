using System;
using UnityEngine;

[AddComponentMenu("Components/Health Component")]
public class HealthComponent : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;
    public event EventHandler OnDeathEvent;
    public event Action<float> OnHealthChangedEvent = delegate {};

    public GameObject healthBarPrefab;

    public void AddOrRemove(float amount)
    {
        var newHp = CurrentHealth + amount;
        if (newHp < 0)
            newHp = 0;
        if (newHp > MaxHealth)
            newHp = MaxHealth;

        CurrentHealth = newHp;
        if (CurrentHealth == 0 && OnDeathEvent != null)
        {
            OnDeathEvent.Invoke(this, EventArgs.Empty);
        }

        OnHealthChangedEvent(CurrentHealth);
    }

    public void Fill()
    {
        CurrentHealth = MaxHealth;
        OnHealthChangedEvent(CurrentHealth);
    }
}
