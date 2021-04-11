using UnityEngine;

[AddComponentMenu("Player/Player")]
public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 Position => GetComponent<Rigidbody2D>().position;

    // Components
    private MovementComponent _movementComponent;
    private InputComponent _input;
    private PlayerWeapon _playerWeapon;
    private HealthComponent _healthComponent;
    private Rigidbody2D _rig;

    public void DealDamageToPlayer(int value)
    {
        _healthComponent.AddOrRemove(-value);
    }

    private void Awake()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _input = GetComponent<InputComponent>();
        _rig = GetComponent<Rigidbody2D>();
        _playerWeapon = GetComponentInChildren<PlayerWeapon>();
        _healthComponent = GetComponent<HealthComponent>();

        _input.OnInventoryMoveRight += _playerWeapon.SwitchToRightWeapon;
        _input.OnInventoryMoveLeft += _playerWeapon.SwitchToLeftWeapon;
        _input.OnAttack += _playerWeapon.Use;

        _movementComponent.moveSpeed = moveSpeed;

        _healthComponent.OnDeathEvent += OnPlayerDeath;
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {
        Debug.Log("The player died");
    }

    private void OnDestroy()
    {
        if (_healthComponent != null)
            _healthComponent.OnDeathEvent -= OnPlayerDeath;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
    }

    void Update()
    {
        int x = Mathf.FloorToInt(transform.position.x);
        int y = Mathf.FloorToInt(transform.position.y);
    }
}
