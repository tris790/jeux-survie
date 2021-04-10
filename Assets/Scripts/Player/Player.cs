using UnityEngine;

[AddComponentMenu("Player/Player")]
public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 Position => GetComponent<Rigidbody2D>().position;

    private MovementComponent _movementComponent;
    private HealthComponent _healthComponent;

    public void DealDamageToPlayer(int value)
    {
        _healthComponent.AddOrRemove(-value);
    }

    private void Awake()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _healthComponent = GetComponent<HealthComponent>();

        _movementComponent.moveSpeed = moveSpeed;
        _healthComponent.OnDeathEvent += OnPlayerDeath;
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {
        Debug.Log("The player died");
    }
}
