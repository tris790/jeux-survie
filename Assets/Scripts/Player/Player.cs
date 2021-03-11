using UnityEngine;

[AddComponentMenu("Player/Player")]
public class Player : MonoBehaviour
{
    // TODO : EGC add this in a stats class
    public float moveSpeed;

    private MovementComponent _movement;

    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<MovementComponent>();

        _movement.moveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
