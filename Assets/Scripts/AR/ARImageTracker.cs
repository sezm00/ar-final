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

    private Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (imageManager == null)
        {
            imageManager = GetComponent<ARTrackedImageManager>();
        }

        foreach (var entry in imagePrefabs)
        {
            if (entry != null && entry.prefab != null && !prefabLookup.ContainsKey(entry.imageName))
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
            HandleImageAdded(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            HandleImageUpdated(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            HandleImageRemoved(trackedImage);
        }
    }

    private void HandleImageAdded(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (prefabLookup.TryGetValue(imageName, out GameObject prefab))
        {
            GameObject spawnedContent = Instantiate(
                prefab,
                trackedImage.transform.position,
                trackedImage.transform.rotation,
                trackedImage.transform
            );

            spawnedContent.name = imageName + "_Spawned";
        }
        else
        {
            Debug.LogWarning("No prefab assigned for image: " + imageName);
        }
    }

    private void HandleImageUpdated(ARTrackedImage trackedImage)
    {
        if (trackedImage.transform.childCount == 0)
        {
            return;
        }

        GameObject content = trackedImage.transform.GetChild(0).gameObject;

        switch (trackedImage.trackingState)
        {
            case TrackingState.Tracking:
                content.SetActive(true);
                break;

            case TrackingState.Limited:
                content.SetActive(true);
                break;

            case TrackingState.None:
                content.SetActive(false);
                break;
        }
    }

    private void HandleImageRemoved(ARTrackedImage trackedImage)
    {
        Debug.Log("Image removed: " + trackedImage.referenceImage.name);
    }
}