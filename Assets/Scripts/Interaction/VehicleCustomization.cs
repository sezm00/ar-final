using UnityEngine;

public class VehicleCustomization : MonoBehaviour
{
    [Header("Drag your materials here")]
    public Material[] bodyColors;
    public Renderer bodyRenderer;
    private int currentColorIndex = 0;

    [Header("Wheel options")]
    public GameObject[] wheelPrefabs;
    public Transform[] wheelPositions;
    private int currentWheelIndex = 0;
    private GameObject[] currentWheels;

    public void ChangeColor()
    {
        if (bodyColors.Length == 0)
        {
            Debug.LogWarning("No body colors assigned!");
            return;
        }
        if (bodyRenderer == null)
        {
            Debug.LogWarning("No body renderer assigned!");
            return;
        }

        currentColorIndex = (currentColorIndex + 1) % bodyColors.Length;
        bodyRenderer.material = bodyColors[currentColorIndex];
        Debug.Log("Color changed to index: " + currentColorIndex);
    }

    public void ChangeWheels()
    {
        if (wheelPrefabs.Length == 0)
        {
            Debug.LogWarning("No wheel prefabs assigned!");
            return;
        }
        if (wheelPositions.Length == 0)
        {
            Debug.LogWarning("No wheel positions assigned!");
            return;
        }

        if (currentWheels != null)
        {
            foreach (GameObject wheel in currentWheels)
            {
                if (wheel != null) Destroy(wheel);
            }
        }

        currentWheelIndex = (currentWheelIndex + 1) % wheelPrefabs.Length;
        currentWheels = new GameObject[wheelPositions.Length];

        for (int i = 0; i < wheelPositions.Length && i < wheelPrefabs.Length; i++)
        {
            if (wheelPrefabs[currentWheelIndex] != null && wheelPositions[i] != null)
            {
                currentWheels[i] = Instantiate(wheelPrefabs[currentWheelIndex], wheelPositions[i]);
                currentWheels[i].transform.localPosition = Vector3.zero;
                currentWheels[i].transform.localRotation = Quaternion.identity;
            }
        }
    }
}