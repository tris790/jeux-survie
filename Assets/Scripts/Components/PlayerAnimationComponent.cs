using UnityEngine;

[AddComponentMenu("Components/Player Animation Component")]
public class PlayerAnimationComponent : MonoBehaviour
{
    // Components
    private Animator _anim;

    // Is called after all objects are initialized
    private void Awake()
    {
        _anim = GetComponent<Animator>();

        // Subscribe to MovementComponents AnimateMovement delegate
        GetComponent<MovementComponent>().AnimateMovement += AnimateMovement;
    }

    void AnimateMovement(bool isMoving)
    {
        _anim.SetBool("moving", isMoving);
    }
}
