using System;
using UnityEngine;

[AddComponentMenu("Components/Health Component")]
public class HealthComponent : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;
    public event EventHandler OnDeathEvent;
    public event Action<float> OnHealthChangedEvent = delegate {};

    public GameObject _healthBarPrefab;
    private SpriteRenderer _spriteRenderer;
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
            _healthBarPrefab.SetActive(false);
            OnDeathEvent.Invoke(this, EventArgs.Empty);
        }

        OnHealthChangedEvent(CurrentHealth);
    }

    private void Awake()
    {
        if (_healthBarPrefab == null)
        {
            _healthBarPrefab = ObjectPoolManager.Instance.GetNextPooledObjectByTag("HealthBarMonster");
            _healthBarPrefab.SetActive(true);
        }

        _spriteRenderer = _healthBarPrefab.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.green;
    }

    private void OnGUI()
    {
        var newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1);
        _healthBarPrefab.transform.position = newPosition;
        if (_isDirty)
            _spriteRenderer.color = Color.Lerp(Color.red, Color.green, CurrentHealth / MaxHealth);
    }

    private void OnDestroy()
    {
        if (_healthBarPrefab != null)
            _healthBarPrefab.SetActive(false);
    }
}
