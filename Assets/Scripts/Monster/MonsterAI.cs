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

    public float aiInterval = 0.5f;
    public SpriteRenderer sprite;
    public int damage = 5;

    private MonsterState _state;
    private Player _player;
    private HealthComponent _targetHealthComponent;
    private MonsterMovementComponent _movementComponent;
    private HealthComponent _healthComponent;

    public void Initialize()
    {
        _state = MonsterState.Following;
        aiInterval = 0.5f;

        _healthComponent.Fill();
        StopMoving();

        _player = GameManager.Instance.player.GetComponent<Player>();
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
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        RotateTowardPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _targetHealthComponent = _player.GetComponent<HealthComponent>();
            _state = MonsterState.Attacking;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _state = MonsterState.Following;
    }

    private void AILoop()
    {
        switch (_state)
        {
            case MonsterState.Idle:
                {
                    break;
                }
            case MonsterState.Roaming:
                {
                    break;
                }
            case MonsterState.Following:
                {
                    MoveTowardPlayer();
                    break;
                }
            case MonsterState.Attacking:
                {
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
