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
        this.transform.parent.GetComponent<MovementComponent>().AnimateMovement += AnimateMovement;
        this.transform.parent.GetComponent<InputComponent>().OnAttack += AnimateAttack;
    }

    void AnimateMovement(bool isMoving)
    {
        _anim.SetBool("moving", isMoving);
    }

    void AnimateAttack()
    {
        _anim.SetTrigger("fire");
    }

    public void Animate()
    {
        this.transform.parent.GetComponent<MovementComponent>().AnimateMovement += AnimateMovement;
    }
}
