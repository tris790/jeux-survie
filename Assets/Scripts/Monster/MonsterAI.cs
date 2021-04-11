using UnityEngine;

public enum MonsterState
{
    Idle,
    Roaming,
    Following,
    Attacking
}

[AddComponentMenu("Monster/Monster Ai")]
public class MonsterAI : MonoBehaviour, IInitialized
{
    public MonsterState state = MonsterState.Roaming;
    public float aiInterval = 0.5f;
    public SpriteRenderer sprite;
    public int damage = 5;

    private Player _player;
    private HealthComponent _targetHealthComponent;
    private MonsterMovementComponent _movementComponent;
    private HealthComponent _healthComponent;

    public void Initialize()
    {
        state = MonsterState.Roaming;
        aiInterval = 0.5f;
        damage = 5;

        _healthComponent.Fill();
        StopMoving();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AILoop), aiInterval, aiInterval);
    }

    private void Awake()
    {
        _movementComponent = GetComponent<MonsterMovementComponent>();
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnDeathEvent += OnMonsterDeath;
    }

    private void OnMonsterDeath(object sender, System.EventArgs e)
    {
        Debug.Log("Monster died");
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        RotateTowardPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CurrentPlayerModel"))
        {
            _player = collision.gameObject.GetComponentInParent<Player>();
            _targetHealthComponent = _player.GetComponent<HealthComponent>();
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
        if (_targetHealthComponent)
            _targetHealthComponent.AddOrRemove(-damage);
    }

    private void RotateTowardPlayer()
    {
        if (_movementComponent != null && _player != null)
        {
            float angle = Mathf.Atan2(
                _player.Position.y - _movementComponent.Position.y,
                _player.Position.x - _movementComponent.Position.x) * Mathf.Rad2Deg;

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
        if (_movementComponent != null && _player != null)
        {
            _movementComponent.moveDirection = new Vector2(
                _player.Position.x - _movementComponent.Position.x,
                _player.Position.y - _movementComponent.Position.y);
        }
    }
}
