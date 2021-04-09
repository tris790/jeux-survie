using UnityEngine;

[AddComponentMenu("Player/Player")]
public class Player : MonoBehaviour, IPositionInWorld
{
    // TODO : EGC add this in a stats class
    public float moveSpeed;

    // Components
    private MovementComponent _movement;
    public Vector2 position;

    private void Update()
    {
        position = _movement.rigidbody.position;
    }

    // Is called after all objects are initialized
    private void Awake()
    {
        _movement = GetComponent<MovementComponent>();

        _movement.moveSpeed = moveSpeed;
        position = _movement.rigidbody.position;
    }
}
