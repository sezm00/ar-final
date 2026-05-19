using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager Instance;

    public GameObject currentVehicle;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnVehicle(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (currentVehicle != null)
        {
            Destroy(currentVehicle);
        }

        currentVehicle = Instantiate(prefab, position, rotation);
    }
}