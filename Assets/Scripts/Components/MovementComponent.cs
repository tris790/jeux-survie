using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

[AddComponentMenu("Components/Movement Component")]
public class MovementComponent : MonoBehaviour, IMoveable, IRotable
{
    public event Action<bool> AnimateMovement = delegate { };

    [HideInInspector]
    public float moveSpeed;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private bool _isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
        AnimateMovement(_isMoving);
        Rotate();
    }

    // TODO : EGC move this in a InputComponent
    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _isMoving = (moveX != 0 || moveY != 0);
        _moveDirection = new Vector2(moveX, moveY);
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
