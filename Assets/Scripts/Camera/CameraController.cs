using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    public GameObject _objectToFollow;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        float charPosX = _objectToFollow.transform.position.x;
        float charPosZ = _objectToFollow.transform.position.z - 1;
        float cameraOffset = _objectToFollow.transform.position.y;

        _camera.transform.position = new Vector3(charPosX, cameraOffset, charPosZ);
    }
}
