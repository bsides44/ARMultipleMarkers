using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARTrackedImageManager))]

public class multipleImageTracker : MonoBehaviour
{
    public GameObject placedPrefabOne;
    public GameObject placedPrefabTwo;
    public GameObject placedPrefabThree;
    public GameObject placedPrefabFour;
    public GameObject placedPrefabFive;
    public Camera arCamera;
    public Text scoreText;
    private GameObject spawnedObjectOne;
    private GameObject spawnedObjectTwo;
    private GameObject spawnedObjectThree;
    private GameObject spawnedObjectFour;
    private GameObject spawnedObjectFive;
    private bool timerIsRunning = false;
    private float targetTime = 4;
    private string markerOutOfViewName;
    private Touch touch;
    private Vector3 touchPosition;
    private int scoreCount;
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
            updateObjectPosition(trackedImage);
        }
        // When our tracked image has disappeared from frame - never gets called because ARKit / ARCore have taken over object tracking
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (trackedImage.referenceImage.name == placedPrefabOne.name) spawnedObjectOne.SetActive(false);
            if (trackedImage.referenceImage.name == placedPrefabTwo.name) spawnedObjectTwo.SetActive(false);
            if (trackedImage.referenceImage.name == placedPrefabThree.name) spawnedObjectThree.SetActive(false);
            if (trackedImage.referenceImage.name == placedPrefabFour.name) spawnedObjectFour.SetActive(false);
            if (trackedImage.referenceImage.name == placedPrefabFive.name) spawnedObjectFive.SetActive(false);
        }
    }

    // IEnumerator disappearObject(ARTrackedImage trackedImage){
    //     if (startTimer){
    //         startTimer = false;
    //         yield return new WaitForSeconds(2.0f);
    //         if (trackedImage.referenceImage.name == placedPrefabOne.name) spawnedObjectOne.SetActive(false);
    //         if (trackedImage.referenceImage.name == placedPrefabTwo.name) spawnedObjectTwo.SetActive(false);
    //     }
    //     startTimer = true;
    // }

    void Update()
    {

        // touch manager
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }

        // Touch object on screen
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(touchPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                if (hit.collider.tag == "Touchable")
                {
                    scoreCount++;
                    string hitName = hit.collider.gameObject.name;

                    if (hitName == placedPrefabOne.name + "(Clone)") spawnedObjectOne.SetActive(false);
                    if (hitName == placedPrefabTwo.name + "(Clone)") spawnedObjectTwo.SetActive(false);
                    if (hitName == placedPrefabThree.name + "(Clone)") spawnedObjectThree.SetActive(false);
                    if (hitName == placedPrefabFour.name + "(Clone)") spawnedObjectFour.SetActive(false);
                    if (hitName == placedPrefabFive.name + "(Clone)") spawnedObjectFive.SetActive(false);
                }
            }
        }

        scoreText.text = "Score: " + scoreCount;

        // marker out of view timeout
        if (timerIsRunning)
        {
            targetTime -= Time.deltaTime;
            if (targetTime <= 0.0f)
            {
                timerIsRunning = false;
                if (markerOutOfViewName == placedPrefabOne.name) spawnedObjectOne.SetActive(false);
                if (markerOutOfViewName == placedPrefabTwo.name) spawnedObjectTwo.SetActive(false);
                if (markerOutOfViewName == placedPrefabThree.name) spawnedObjectThree.SetActive(false);
                if (markerOutOfViewName == placedPrefabFour.name) spawnedObjectFour.SetActive(false);
                if (markerOutOfViewName == placedPrefabFive.name) spawnedObjectFive.SetActive(false);

                targetTime = 4;
            }
        }
    }

    void limitedTrackingTimeout(string trackedImageName)
    {
        markerOutOfViewName = trackedImageName;
        timerIsRunning = true;
    }

    void putObjectOnImage(ARTrackedImage trackedImage)
    {
        string trackedImageName = trackedImage.referenceImage.name;

        if (trackedImageName == placedPrefabOne.name)
        {
            spawnedObjectOne = Instantiate(placedPrefabOne, trackedImage.transform.position, placedPrefabOne.transform.rotation);
        }
        if (trackedImageName == placedPrefabTwo.name)
        {
            spawnedObjectTwo = Instantiate(placedPrefabTwo, trackedImage.transform.position, placedPrefabTwo.transform.rotation);
        }
        if (trackedImageName == placedPrefabThree.name)
        {
            spawnedObjectThree = Instantiate(placedPrefabThree, trackedImage.transform.position, placedPrefabThree.transform.rotation);
        }
        if (trackedImageName == placedPrefabFour.name)
        {
            spawnedObjectFour = Instantiate(placedPrefabFour, trackedImage.transform.position, placedPrefabFour.transform.rotation);
        }
        if (trackedImageName == placedPrefabFive.name)
        {
            spawnedObjectFive = Instantiate(placedPrefabFive, trackedImage.transform.position, placedPrefabFive.transform.rotation);
        }
    }

    void updateObjectPosition(ARTrackedImage trackedImage)
    {
        string trackedImageName = trackedImage.referenceImage.name;

        if (trackedImageName == placedPrefabOne.name)
        {
            spawnedObjectOne.transform.rotation = trackedImage.transform.rotation;
            spawnedObjectOne.transform.position = trackedImage.transform.position;
            spawnedObjectOne.SetActive(true);
        }
        if (trackedImageName == placedPrefabTwo.name)
        {
            spawnedObjectTwo.transform.rotation = trackedImage.transform.rotation;
            spawnedObjectTwo.transform.position = trackedImage.transform.position;
            spawnedObjectTwo.SetActive(true);
        }
        if (trackedImageName == placedPrefabThree.name)
        {
            spawnedObjectThree.transform.rotation = trackedImage.transform.rotation;
            spawnedObjectThree.transform.position = trackedImage.transform.position;
            spawnedObjectThree.SetActive(true);
        }
        if (trackedImageName == placedPrefabFour.name)
        {
            spawnedObjectFour.transform.rotation = trackedImage.transform.rotation;
            spawnedObjectFour.transform.position = trackedImage.transform.position;
            spawnedObjectFour.SetActive(true);
        }
        if (trackedImageName == placedPrefabFive.name)
        {
            spawnedObjectFive.transform.rotation = trackedImage.transform.rotation;
            spawnedObjectFive.transform.position = trackedImage.transform.position;
            spawnedObjectFive.SetActive(true);
        }
        // deactivate object when marker disappears from view for more than 4 seconds
        if (trackedImage.trackingState != TrackingState.Tracking && !timerIsRunning)
        {
            limitedTrackingTimeout(trackedImageName);
        }
    }
}
