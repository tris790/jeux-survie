using UnityEngine;

[AddComponentMenu("Components/Input Component")]
public class InputComponent : MonoBehaviour
{
    public float Horizontal { get; set; }
    public float Vertical { get; set; }

    // TODO : EGC There will be several event action delegate like OnAttack, OnInteract ...

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    public void ProcessInputs()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
    }
}
