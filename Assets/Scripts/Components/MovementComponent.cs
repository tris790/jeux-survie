using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

[AddComponentMenu("Components/Movement Component")]
public class MovementComponent : MonoBehaviour, IMoveable, IRotable
{
    [HideInInspector]
    public float moveSpeed;

    // Events
    public event Action<bool> AnimateMovement = delegate { };

    // Components
    private Rigidbody2D _rigidbody;
    private InputComponent _input;

    private Vector2 _moveDirection;
    private bool _isMoving = false;

    // Is called after all objects are initialized
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        _isMoving = (_input.Horizontal != 0 || _input.Vertical != 0);
        _moveDirection = new Vector2(_input.Horizontal, _input.Vertical);
    }

    void FixedUpdate()
    {
        Move();
        AnimateMovement(_isMoving);
        Rotate();
    }

    public void Move()
    {
        _rigidbody.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    public void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
