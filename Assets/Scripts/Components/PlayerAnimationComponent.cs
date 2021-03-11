using UnityEngine;

[AddComponentMenu("Components/Player Animation Component")]
public class PlayerAnimationComponent : MonoBehaviour
{
    private Animator _anim;

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
