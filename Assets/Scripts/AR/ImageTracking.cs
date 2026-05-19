using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageTracking : MonoBehaviour
{
    public GameObject dodgePrefab;
    public GameObject chevyPrefab;

    private ARTrackedImageManager trackedImageManager;

    private Dictionary<string, GameObject> spawnedPrefabs =
        new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        GameObject prefab = null;

        switch (imageName)
        {
            case "Dodge":
                prefab = dodgePrefab;
                break;

            case "Chevy":
                prefab = chevyPrefab;
                break;
        }

        if (prefab == null)
            return;

        if (!spawnedPrefabs.ContainsKey(imageName))
        {
            GameObject obj = Instantiate(
                prefab,
                trackedImage.transform.position,
                trackedImage.transform.rotation
            );

            spawnedPrefabs.Add(imageName, obj);
        }
        else
        {
            GameObject obj = spawnedPrefabs[imageName];

            obj.transform.position = trackedImage.transform.position;
            obj.transform.rotation = trackedImage.transform.rotation;
        }
    }
}