using UnityEngine;

public class followCharacter : MonoBehaviour
{
    [SerializeField] public Camera mainCamera;
    private Vector3 pos;

    void Start()
    {
        mainCamera = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos.z = gameObject.transform.position.z - 7;
        pos.x = mainCamera.transform.position.x;
        pos.y = mainCamera.transform.position.y;
        mainCamera.transform.position = pos;
    }
}
