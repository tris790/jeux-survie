using UnityEngine;

[AddComponentMenu("Player/Player")]
public class Player : MonoBehaviour
{
    // TODO : EGC add this in a stats class
    public float moveSpeed;

    // Components
    private MovementComponent _movement;

    // Is called after all objects are initialized
    private void Awake()
    {
        _movement = GetComponent<MovementComponent>();

        _movement.moveSpeed = moveSpeed;
    }
}
