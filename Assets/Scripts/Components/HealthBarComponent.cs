using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Components/HealthBar Component")]
public class HealthBarComponent : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _healthComponent = gameObject.GetComponentInParent<HealthComponent>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.green;
    }

    private void OnGUI()
    {
        //var newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -5);
        //gameObject.transform.position = newPosition; 
        _spriteRenderer.color = Color.Lerp(Color.red, Color.green, _healthComponent.CurrentHealth / _healthComponent.MaxHealth);
    }
}
