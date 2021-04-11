using System;
using UnityEngine;

[AddComponentMenu("Components/Input Component")]
public class InputComponent : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float scroll;

    public event Action OnAttack = delegate { };
    public event Action AnimateAttack = delegate { };
    public event Action OnInventoryMoveLeft = delegate { };
    public event Action OnInventoryMoveRight = delegate { };

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

        if (scroll < 0f)
        {
            OnInventoryMoveLeft();
        }

        if (scroll > 0f)
        {
            OnInventoryMoveRight();
        }

        // Left mouse click 
        if (Input.GetMouseButton(0))
        {
            OnAttack();
        }
    }

    public void ProcessInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        scroll = Input.GetAxis("Mouse ScrollWheel");
    }
}
