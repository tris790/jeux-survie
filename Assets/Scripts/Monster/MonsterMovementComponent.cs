using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

[AddComponentMenu("Components/Monster Movement Component")]
public class MonsterMovementComponent : MonoBehaviour
{
    public bool isMoving;
    public float moveSpeed;
    public event Action<bool> AnimateMovement = delegate { };

    public Vector2 Position => transform.position;
    public Vector2 moveDirection;

    private Rigidbody2D _rigidbody;
        
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Rotate(Quaternion angle)
    {
        transform.rotation = angle;
        _rigidbody.transform.rotation = angle;
    }

    public void Move()
    {
        _rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void TeleportTo(Vector2 to)
    {
        _rigidbody.position = to;
    }

    private void FixedUpdate()
    {
        Move();
        AnimateMovement(isMoving);
    }
}