using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[Serializable]
public class ImagePrefabEntry
{
    public string imageName;
    public GameObject prefab;
}

public class ARImageTracker : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager imageManager;

    [SerializeField] private List<ImagePrefabEntry> imagePrefabs;

    private Dictionary<string, GameObject> prefabLookup =
        new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> spawnedObjects =
        new Dictionary<string, GameObject>();

    private void Awake()
    {

        foreach (var entry in imagePrefabs)
        {
            if (!prefabLookup.ContainsKey(entry.imageName))
            {
                prefabLookup.Add(entry.imageName, entry.prefab);
            }
        }
    }

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            SpawnPrefab(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdatePrefab(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            string imageName = trackedImage.referenceImage.name;

            if (spawnedObjects.ContainsKey(imageName))
            {
                Destroy(spawnedObjects[imageName]);
                spawnedObjects.Remove(imageName);
            }

            Debug.Log("Removed image: " + imageName);
        }
    }

    private void SpawnPrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!prefabLookup.ContainsKey(imageName))
        {
            Debug.LogWarning("No prefab assigned for: " + imageName);
            return;
        }

        if (spawnedObjects.ContainsKey(imageName))
            return;

        GameObject prefab = prefabLookup[imageName];

        GameObject spawnedObject = Instantiate(
            prefab,
            trackedImage.transform.position,
            trackedImage.transform.rotation
        );

        spawnedObject.transform.SetParent(trackedImage.transform);

        spawnedObjects.Add(imageName, spawnedObject);

        Debug.Log("Spawned prefab for: " + imageName);
    }

    private void UpdatePrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!spawnedObjects.ContainsKey(imageName))
            return;

        GameObject spawnedObject = spawnedObjects[imageName];

        spawnedObject.transform.position = trackedImage.transform.position;
        spawnedObject.transform.rotation = trackedImage.transform.rotation;

        switch (trackedImage.trackingState)
        {
            case TrackingState.Tracking:
                spawnedObject.SetActive(true);
                break;

            case TrackingState.Limited:
                spawnedObject.SetActive(true);
                break;

            case TrackingState.None:
                spawnedObject.SetActive(false);
                break;
        }
    }
}