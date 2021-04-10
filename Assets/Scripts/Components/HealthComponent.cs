using System;
using UnityEngine;

[AddComponentMenu("Components/Health")]
public class HealthComponent : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;
    public event EventHandler OnDeathEvent;

    private GameObject healthBarPrefab;
    private SpriteRenderer spriteRenderer;
    private bool _isDirty = true;

    public void AddOrRemove(int amount)
    {
        _isDirty = true;
        var newHp = CurrentHealth + amount;
        if (newHp < 0)
            newHp = 0;

        CurrentHealth = newHp;
        if (CurrentHealth == 0 && OnDeathEvent != null)
        {
            healthBarPrefab.SetActive(false);
            OnDeathEvent.Invoke(this, EventArgs.Empty);
        }
    }

    private void Awake()
    {
        if (healthBarPrefab == null)
        {
            healthBarPrefab = ObjectPoolManager.Instance.GetNextPooledObjectByTag("HealthBarMonster");
            healthBarPrefab.SetActive(true);
        }

        spriteRenderer = healthBarPrefab.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;
    }

    private void OnGUI()
    {
        var newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1);
        healthBarPrefab.transform.position = newPosition;
        if (_isDirty)
            spriteRenderer.color = Color.Lerp(Color.red, Color.green, CurrentHealth / MaxHealth);
    }
}
