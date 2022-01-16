using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class PlaceObjectOnPlanes : MonoBehaviour
{
    public GameObject placementIndicator;
    
    Pose placementPose;
    Transform placementTransform;
    bool placementPoseIsValid = false;
    TrackableId placePlaneId = TrackableId.invalidId;

    ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();        
    }
    void Update()
    {
        UpdatePlacementIndicator();
        UpdatePlacementPosition();
    }

    void UpdatePlacementPosition()
    {
        if (m_RaycastManager.Raycast(Camera.main.ViewportToScreenPoint(new Vector3(0.5f,0.5f)),s_Hits,TrackableType.PlaneWithinPolygon))
        {
            if (placementPoseIsValid)
            {
                placementPose = s_Hits[0].pose;
                placePlaneId = s_Hits[0].trackableId;

                var planManager = GetComponent<ARPlaneManager>();
                ARPlane arPlane = planManager.GetPlane(placePlaneId);
                placementTransform = arPlane.transform;
            }
        }
    }

    void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementTransform.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
}
