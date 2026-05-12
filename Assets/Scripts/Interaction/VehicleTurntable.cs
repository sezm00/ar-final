using UnityEngine;

public class VehicleTurntable : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 30f;
    private bool isRotating = false;

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void ToggleRotation()
    {
        isRotating = !isRotating;
        Debug.Log("Auto-rotate: " + (isRotating ? "ON" : "OFF"));
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public bool IsRotating()
    {
        return isRotating;
    }
}