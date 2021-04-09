using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

[AddComponentMenu("Components/Monster Movement Component")]
public class MonsterMovementComponent : MonoBehaviour, IMoveable, IRotable
{
    public bool isMoving;
    public float moveSpeed;
    public event Action<bool> AnimateMovement = delegate { };
    public Rigidbody2D rigidbody;
    public Vector2 moveDirection;

    public void Move()
    {
        rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void Rotate()
    {
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Move();
        AnimateMovement(isMoving);
        Rotate();
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
}