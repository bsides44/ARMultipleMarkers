using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]

public class doubleARImageTracker : MonoBehaviour
{
    public GameObject placedPrefabOne;
    public GameObject placedPrefabTwo;
    private GameObject spawnedObjectOne;
    private GameObject spawnedObjectTwo;
   
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

    // When this GameObject dies, Remove the subscription to run "trackedImageUpdates" whenever trackedImagesChanged is called.
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
            if (trackedImage.referenceImage.name == placedPrefabOne.name) spawnedObjectOne.SetActive(false);
            if (trackedImage.referenceImage.name == placedPrefabTwo.name) spawnedObjectTwo.SetActive(false);
        }
    }

    void putObjectOnImage(ARTrackedImage trackedImage)
    {
        string trackedImageName = trackedImage.referenceImage.name;

        if (trackedImageName == placedPrefabOne.name)
        {
            if (spawnedObjectOne == null)
            {
                spawnedObjectOne = Instantiate(placedPrefabOne, trackedImage.transform.position, placedPrefabOne.transform.rotation);
            }
            else {
                spawnedObjectOne.transform.rotation = trackedImage.transform.rotation;
                spawnedObjectOne.transform.position = trackedImage.transform.position;
            }
        }
        if (trackedImageName == placedPrefabTwo.name)
        {
            if (spawnedObjectTwo == null)
            {
            spawnedObjectTwo = Instantiate(placedPrefabTwo, trackedImage.transform.position, placedPrefabTwo.transform.rotation);
            }
            else {
            spawnedObjectTwo.transform.rotation = trackedImage.transform.rotation;
            spawnedObjectTwo.transform.position = trackedImage.transform.position;
            }
        }
    }
}
