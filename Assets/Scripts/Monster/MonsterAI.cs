using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle,
    Roaming,
    Following,
    Attacking
}

[AddComponentMenu("Monster/Monster Ai")]
public class MonsterAI : MonoBehaviour
{
    public float attackRange;
    public MonsterState state = MonsterState.Roaming;
    public MonsterMovementComponent _movementComponent;
    public float aiInterval = 0.5f;
    public SpriteRenderer sprite;
    public int damage = 5;

    public MovementComponent targetMovementComponent;
    public HealthComponent targetHealthComponent;

    private void Start()
    {
        InvokeRepeating(nameof(AILoop), aiInterval, aiInterval);
    }

    private void FixedUpdate()
    {
        RotateTowardPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetMovementComponent = collision.gameObject.GetComponent<MovementComponent>();
            targetHealthComponent = collision.gameObject.GetComponent<HealthComponent>();
            state = MonsterState.Attacking;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        state = MonsterState.Following;
    }

    private void AILoop()
    {
        switch (state)
        {
            case MonsterState.Idle:
                {
                    sprite.color = Color.white;
                    break;
                }
            case MonsterState.Roaming:
                {
                    sprite.color = Color.blue;
                    break;
                }
            case MonsterState.Following:
                {
                    sprite.color = Color.green;
                    MoveTowardPlayer();
                    break;
                }
            case MonsterState.Attacking:
                {
                    sprite.color = Color.red;
                    StopMoving();
                    Attack();
                    break;
                }
            default:
                break;
        }
    }

    private void Attack()
    {
        if (targetHealthComponent)
            targetHealthComponent.AddOrRemove(-damage);
    }

    private void Awake()
    {
        _movementComponent = GetComponent<MonsterMovementComponent>();
    }

    private void RotateTowardPlayer()
    {
        if (_movementComponent != null && targetMovementComponent != null)
        {
            float angle = Mathf.Atan2(
                targetMovementComponent.transform.position.y - _movementComponent.Position.y,
                targetMovementComponent.transform.position.x - _movementComponent.Position.x) * Mathf.Rad2Deg;

            var quaternionAngle = Quaternion.Euler(new Vector3(0, 0, angle));
            _movementComponent.Rotate(quaternionAngle);
        }
    }

    private void StopMoving()
    {
        if (_movementComponent != null)
            _movementComponent.moveDirection = new Vector2();
    }

    private void MoveTowardPlayer()
    {
        if (_movementComponent != null && targetMovementComponent != null)
        {
            _movementComponent.moveDirection = new Vector2(
                targetMovementComponent.transform.position.x - _movementComponent.Position.x,
                targetMovementComponent.transform.position.y - _movementComponent.Position.y);
        }
    }
}
