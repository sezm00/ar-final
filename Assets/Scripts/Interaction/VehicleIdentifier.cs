using UnityEngine;

public class VehicleIdentifier : MonoBehaviour
{
    [Header("Which car is this?")]
    public int carIndex = 0; // 0 = first car, 1 = second car

    [Header("Voiceover clip for this car")]
    public int voiceoverClipIndex = 0; // matches index in AudioManager

    void Start()
    {
        // Set tag so UI can find this vehicle
        gameObject.tag = "Vehicle";
    }

    public int GetCarIndex()
    {
        return carIndex;
    }
}