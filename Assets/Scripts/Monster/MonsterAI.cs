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
    public Player playerEntity;
    public float aiInterval = 0.5f;
    public SpriteRenderer sprite;

    void Start()
    {
        //InvokeRepeating(nameof(AILoop), aiInterval, aiInterval);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        AILoop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        state = MonsterState.Attacking;
        Debug.Log("Trigger1");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        state = MonsterState.Following;
        Debug.Log("Stop trigger2");
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
                    RotateTowardPlayer();
                    break;
                }
            case MonsterState.Attacking:
                {
                    sprite.color = Color.red;
                    StopMoving();
                    Attack();
                    RotateTowardPlayer();
                    break;
                }
            default:
                break;
        }
    }

    private void Attack()
    {
    }

    private void Awake()
    {
        _movementComponent = GetComponent<MonsterMovementComponent>();
    }

    private void RotateTowardPlayer()
    {
        float angle = Mathf.Atan2(playerEntity.position.y -_movementComponent.Position.y, playerEntity.position.x -_movementComponent.Position.x) * Mathf.Rad2Deg;
        var quaternionAngle = Quaternion.Euler(new Vector3(0, 0, angle));
        _movementComponent.Rotate(quaternionAngle);
    }

    private void StopMoving()
    {
        _movementComponent.moveDirection = new Vector2();
    }

    private void MoveTowardPlayer()
    {
        _movementComponent.moveDirection = new Vector2(playerEntity.position.x - _movementComponent.Position.x, playerEntity.position.y - _movementComponent.Position.y);
    }
}
