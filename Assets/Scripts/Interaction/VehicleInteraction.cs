using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    private Camera cam;

    [Header("Rotation")]
    public float rotationSpeed = 0.2f;

    [Header("Scaling")]
    public float scaleSpeed = 0.01f;
    public float minScale = 0.5f;
    public float maxScale = 2f;

    void Start()
    {
        cam = Camera.main;
    }



    void Update()
    {
        // One finger = rotate
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float rot = touch.deltaPosition.x * rotationSpeed;
                transform.Rotate(0, -rot, 0);
            }
        }

        // Two fingers = scale
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            float prevDist = (t1.position - t1.deltaPosition - (t2.position - t2.deltaPosition)).magnitude;
            float currDist = (t1.position - t2.position).magnitude;

            float diff = currDist - prevDist;

            Vector3 scale = transform.localScale + Vector3.one * diff * scaleSpeed;

            scale = Vector3.Max(scale, Vector3.one * minScale);
            scale = Vector3.Min(scale, Vector3.one * maxScale);

            transform.localScale = scale;
        }
    }
}