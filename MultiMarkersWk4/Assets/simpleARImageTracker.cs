using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]

public class simpleARImageTracker : MonoBehaviour
{
    public GameObject placedPrefabOne;
    private GameObject spawnedObjectOne;
    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }
    // AR Tracked Image Manager tracks images that match those in our Reference library

    // When this GameObject comes to life, run "trackedImageUpdates" whenever our AR Tracked Image Manager component releases a "trackedImagesChanged" event. trackedImagesChanged runs once per frame and notes whether our tracked image has been found or lost. 
    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += trackedImageUpdates;
    }

    // When this GameObject dies, remove the subscription to run "trackedImageUpdates" whenever trackedImagesChanged is called.
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= trackedImageUpdates;
    }

    void trackedImageUpdates(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // When our tracked image is found in frame
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            putObjectOnImage(trackedImage);
        }
        // When our tracked image has in some way changed since last frame (ie become obscured)
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            putObjectOnImage(trackedImage);
        }
        // When our tracked image has disappeared from frame - never gets called because ARKit / ARCore have taken over object tracking
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedObjectOne.SetActive(false);
        }
    }

    void putObjectOnImage(ARTrackedImage trackedImage)
    {
        if (spawnedObjectOne == null) 
        {
        spawnedObjectOne = Instantiate(placedPrefabOne, trackedImage.transform.position, placedPrefabOne.transform.rotation);
        }
        else 
        {
            spawnedObjectOne.transform.position = trackedImage.transform.position;
            spawnedObjectOne.transform.rotation = trackedImage.transform.rotation;
        }
    }
}
