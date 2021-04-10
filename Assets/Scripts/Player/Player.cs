using UnityEngine;

[AddComponentMenu("Player/Player")]
public class Player : MonoBehaviour, IPositionInWorld
{
    public float moveSpeed;
    public Vector2 position;

    private MovementComponent _movementComponent;
    private HealthComponent _healthComponent;

    public void DealDamageToPlayer(int value)
    {
        _healthComponent.AddOrRemove(-value);
    }

    private void Update()
    {
        position = _movementComponent.rigidbody.position;
    }

    private void Awake()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _healthComponent = GetComponent<HealthComponent>();

        _movementComponent.moveSpeed = moveSpeed;
        position = _movementComponent.rigidbody.position;
        _healthComponent.OnDeathEvent += OnPlayerDeath;
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {
        Debug.Log("The player died");
    }
}
