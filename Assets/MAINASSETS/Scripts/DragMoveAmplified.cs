using UnityEngine;

public class DragMoveAmplified : MonoBehaviour
{
    public Camera cam;
    public float amplification = 1.2f;   // 1.2 = +20%
    public float worldPerPixel = 0.01f;  // tune this
    public Vector2 xLimits = new Vector2(-4f, 4f);
    public Vector2 zLimits = new Vector2(-8f, 8f);

    bool dragging;
    Vector2 startFinger;
    Vector3 startPos;

    bool PointerDown() =>
    Input.GetMouseButton(0) || Input.touchCount > 0;

    bool PointerBegan() =>
        Input.GetMouseButtonDown(0) ||
        (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

    bool PointerUp() =>
        Input.GetMouseButtonUp(0) ||
        (Input.touchCount > 0 && (
            Input.GetTouch(0).phase == TouchPhase.Ended ||
            Input.GetTouch(0).phase == TouchPhase.Canceled));

    Vector2 PointerPos() =>
        Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        if (!PointerDown()) { dragging = false; return; }

        Vector2 p = PointerPos();

        if (PointerBegan())
        {
            dragging = true;
            startFinger = p;
            startPos = transform.position;
        }
        else if (dragging)
        {
            Vector2 d = (p - startFinger) * amplification;

            Vector3 target = startPos + new Vector3(d.x, 0f, d.y) * worldPerPixel;
            target.x = Mathf.Clamp(target.x, xLimits.x, xLimits.y);
            target.z = Mathf.Clamp(target.z, zLimits.x, zLimits.y);

            transform.position = target;

            if (PointerUp()) dragging = false;
        }
    }
}