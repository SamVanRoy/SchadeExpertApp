using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class ReadyGestureDetect : MonoBehaviour {

    private int index;

    private LineRenderer lineRenderer;
    private GameObject myLine;
    Vector3 positionHand;
    private static float distance = 1.0f;

    public GameObject linePrefab;

    private Vector3 startPos;    // Start position of line
    private Vector3 endPos;    // End position of line

    // Use this for initialization
    void Start () {
        InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;

        StartCoroutine(loadFromResourcesFolder());

        index = 0;

    }

    IEnumerator loadFromResourcesFolder()
    {
        //Request data to be loaded
        ResourceRequest loadAsync = Resources.LoadAsync("Line", typeof(GameObject));

        //Wait till we are done loading
        while (!loadAsync.isDone)
        {
            yield return null;
        }

        //Get the loaded data
        GameObject prefab = loadAsync.asset as GameObject;
        myLine = Instantiate(prefab);
        myLine.transform.position = CalculatePositionWithDistanceInFrontOfCamera(2.0f);
        myLine.AddComponent<LineRenderer>();
        lineRenderer = myLine.GetComponent<LineRenderer>();
        lineRenderer.material.color = Color.red;
        lineRenderer.widthMultiplier = 0.01f;
    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        if (obj.state.source.kind == InteractionSourceKind.Hand)
        {
            if (obj.state.anyPressed)
            {
                obj.state.sourcePose.TryGetPosition(out positionHand);
                DrawLine(positionHand);
                endPos = positionHand;
            }
        }
    }

    private void DrawLine(Vector3 positionHand)
    {
        lineRenderer.positionCount = index+1;

        Vector3 calculatedPositionLine = CalculatePositionObjectInFrontOfCamera(positionHand);
        lineRenderer.SetPosition(index, calculatedPositionLine);
        index++;
    }

    public static Vector3 CalculatePositionObjectInFrontOfCamera(Vector3 positionObject)
    {
        return Camera.main.transform.position + positionObject + Camera.main.transform.forward * distance;
    }

    public static Vector3 CalculatePositionWithDistanceInFrontOfCamera(float distance)
    {
        return Camera.main.transform.position + Camera.main.transform.forward * distance;
    }

    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs obj)
    {
        //vingers in pressed state
        Debug.Log("Source pressed");
    }

    private void InteractionManager_InteractionSourceReleased(InteractionSourceReleasedEventArgs obj)
    {
        //wanneer je vinger terug loslaat van pinch
        myLine.transform.position = CalculatePositionObjectInFrontOfCamera(endPos);
    }

    private void OnDestroy()
    {
        InteractionManager.InteractionSourcePressed -= InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceReleased -= InteractionManager_InteractionSourceReleased;
    }
}
