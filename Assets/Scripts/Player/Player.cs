using UnityEngine;

[AddComponentMenu("Player/Player")]
public class Player : MonoBehaviour
{
    public float moveSpeed;

    // Components
    private MovementComponent _movement;
    private InputComponent _input;
    private Rigidbody2D _rig;
    private PlayerWeapon _playerWeapon;

    // Is called after all objects are initialized
    private void Awake()
    {
        _movement = GetComponent<MovementComponent>();
        _input = GetComponent<InputComponent>();
        _rig = GetComponent<Rigidbody2D>();
        _playerWeapon = GetComponentInChildren<PlayerWeapon>();

        _input.OnInventoryMoveRight += _playerWeapon.SwitchToRightWeapon;
        _input.OnInventoryMoveLeft += _playerWeapon.SwitchToLeftWeapon;

        _input.OnAttack += _playerWeapon.Use;
        _movement.moveSpeed = moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }

    void Update()
    {
        int x = Mathf.FloorToInt(transform.position.x);
        int y = Mathf.FloorToInt(transform.position.y);
    }
}
