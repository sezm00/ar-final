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

    private Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
    private HashSet<TrackableId> spawnedIds = new HashSet<TrackableId>();

    private void Awake()
    {
        if (imageManager == null)
            imageManager = GetComponent<ARTrackedImageManager>();

        foreach (var entry in imagePrefabs)
        {
            if (entry != null && entry.prefab != null &&
                !string.IsNullOrEmpty(entry.imageName) &&
                !prefabLookup.ContainsKey(entry.imageName))
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

    // Poll every frame for any tracked image that has a name but no spawned content yet
    private void Update()
    {
        foreach (ARTrackedImage trackedImage in imageManager.trackables)
        {
            string imageName = trackedImage.referenceImage.name;

            if (string.IsNullOrEmpty(imageName))
                continue;

            if (spawnedIds.Contains(trackedImage.trackableId))
                continue;

            if (prefabLookup.TryGetValue(imageName, out GameObject prefab))
            {
                GameObject spawnedContent = Instantiate(
                    prefab,
                    trackedImage.transform.position,
                    trackedImage.transform.rotation,
                    trackedImage.transform
                );
                spawnedContent.name = imageName + "_Spawned";
                spawnedIds.Add(trackedImage.trackableId);
                Debug.Log("Spawned prefab for image (via poll): " + imageName);
            }
            else
            {
                Debug.LogWarning("No prefab assigned for image: " + imageName);
                // Add anyway to avoid spamming the warning every frame
                spawnedIds.Add(trackedImage.trackableId);
            }
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
            HandleImageAdded(trackedImage);

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
            HandleImageUpdated(trackedImage);

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
            HandleImageRemoved(trackedImage);
    }

    private void HandleImageAdded(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (string.IsNullOrEmpty(imageName))
        {
            Debug.LogWarning("Skipping tracked image with empty name (likely simulation artifact).");
            return;
        }

        SpawnForImage(trackedImage);
    }

    private void HandleImageUpdated(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!string.IsNullOrEmpty(imageName) && !spawnedIds.Contains(trackedImage.trackableId))
        {
            SpawnForImage(trackedImage);
            return;
        }

        if (trackedImage.transform.childCount == 0)
            return;

        GameObject content = trackedImage.transform.GetChild(0).gameObject;

        switch (trackedImage.trackingState)
        {
            case TrackingState.Tracking:
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
        spawnedIds.Remove(trackedImage.trackableId);
        Debug.Log("Image removed: " + trackedImage.referenceImage.name);
    }

    private void SpawnForImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (spawnedIds.Contains(trackedImage.trackableId))
            return;

        if (prefabLookup.TryGetValue(imageName, out GameObject prefab))
        {
            GameObject spawnedContent = Instantiate(
                prefab,
                trackedImage.transform.position,
                trackedImage.transform.rotation,
                trackedImage.transform
            );
            spawnedContent.name = imageName + "_Spawned";
            spawnedIds.Add(trackedImage.trackableId);
            Debug.Log("Spawned prefab for image: " + imageName);
        }
        else
        {
            Debug.LogWarning("No prefab assigned for image: " + imageName);
        }
    }
}