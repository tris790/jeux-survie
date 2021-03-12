using UnityEngine;

[AddComponentMenu("Components/Input Component")]
public class InputComponent : MonoBehaviour
{
    public float horizontal;
    public float vertical;

    // TODO : EGC There will be several event action delegate like OnAttack, OnInteract ...

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    public void ProcessInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
}
