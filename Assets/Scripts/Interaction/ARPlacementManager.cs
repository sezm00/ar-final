using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject prefabToPlace;

    [Header("References")]
    [SerializeField] private ARInputHandler inputHandler;

    private ARRaycastManager _raycastManager;
    private GameObject placedObject;
    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _raycastManager = FindFirstObjectByType<ARRaycastManager>();

        if (_raycastManager == null)
            Debug.LogError("ARRaycastManager not found in scene.");

        if (inputHandler == null)
            Debug.LogError("ARInputHandler not assigned.");
    }

    private void OnEnable()
    {
        inputHandler.OnPerformTap += PlaceObject;
    }

    private void OnDisable()
    {
        inputHandler.OnPerformTap -= PlaceObject;
    }

    private void PlaceObject(Vector2 screenPos)
    {
        if (_raycastManager == null || prefabToPlace == null)
            return;

        if (_raycastManager.Raycast(screenPos, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;

            if (placedObject == null)
            {
                placedObject = Instantiate(prefabToPlace, hitPose.position, hitPose.rotation);
            }
            else
            {
                placedObject.transform.position = hitPose.position;
                placedObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}