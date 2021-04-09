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
    public MonsterMovementComponent movement;
    public Vector2 postion;
    public Player target;
    public float aiInterval = 0.5f;
    public SpriteRenderer sprite;

    void Start()
    {
        InvokeRepeating(nameof(AILoop), aiInterval, aiInterval);
    }

    void Update()
    {
        if (target != null)
        {
            state = MonsterState.Following;
            if (IsWithinAttackRange(target))
            {
                state = MonsterState.Attacking;
            }
        }
        else
        {
            state = MonsterState.Roaming;
        }
    }

    private bool IsWithinAttackRange(Player target)
    {
        return Vector2.Distance(target.position, postion) <= attackRange;
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
                    movement.isMoving = true;
                    Vector2 current = movement.rigidbody.position;
                    Vector2 newDirection = current + new Vector2(UnityEngine.Random.Range(0, 5), UnityEngine.Random.Range(0, 5));
                    movement.moveDirection = newDirection;
                    break;
                }

            case MonsterState.Following:
                {
                    sprite.color = Color.yellow;
                    movement.isMoving = true;
                    Vector2 current = movement.rigidbody.position;
                    Vector2 newDirection = target.position - current;
                    movement.moveDirection = newDirection;
                    break;
                }
            case MonsterState.Attacking:
                {
                    sprite.color = Color.red;
                    movement.isMoving = false;
                    movement.moveDirection = new Vector2(0, 0);

                    Attack();
                }

                break;
            default:
                break;
        }
    }

    private void Attack()
    {
    }

    private void Awake()
    {
        movement = GetComponent<MonsterMovementComponent>();
        postion = movement.rigidbody.position;
    }
}
