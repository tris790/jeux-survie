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
    public Rigidbody2D rigidbody;
    private InputComponent _input;

    private Vector2 _moveDirection;
    private bool _isMoving = false;

    // Is called after all objects are initialized
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        _isMoving = (_input.horizontal != 0 || _input.vertical != 0);
        _moveDirection = new Vector2(_input.horizontal, _input.vertical);
    }

    void FixedUpdate()
    {
        Move();
        AnimateMovement(_isMoving);
        Rotate();
    }

    public void Move()
    {
        rigidbody.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    public void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
