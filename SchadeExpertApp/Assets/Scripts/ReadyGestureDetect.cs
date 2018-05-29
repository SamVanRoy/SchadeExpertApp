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

    private int index1;
    private int index2;

    public Material color;
    private LineRenderer lineRenderer;
    private GameObject myLine;
    Vector3 positionHand;
    private static float distance = 2.0f;

    public GameObject linePrefab;

    private Vector3 startPos;    // Start position of line
    private Vector3 endPos;    // End position of line


    //Vector3[] positions = new Vector3[3];
    // Use this for initialization
    void Start () {
        //InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        //InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;

        StartCoroutine(loadFromResourcesFolder());

        myLine = new GameObject();
        myLine.transform.position = CalculatePositionInFrontOfCamera(new Vector3(0, 0, 0));

        index1 = 0;
        index2 = 0;

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
        myLine.AddComponent<LineRenderer>();
        lineRenderer = myLine.GetComponent<LineRenderer>();
        lineRenderer.material.color = Color.blue;
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
                Debug.Log("pressed: " + positionHand);
                //Debug.Log("position camera: " + Camera.main.transform.position);
                //Debug.Log("possible hand position world: " + Camera.main.transform.TransformPoint(positionHand));
                if (lineRenderer.positionCount == 1)
                {
                    startPos = positionHand;
                    Vector3 adjustedHandposition = new Vector3(positionHand.x, positionHand.y, positionHand.z + 2);
                    lineRenderer.transform.position = adjustedHandposition;
                    Debug.Log("hallo");
                    Debug.Log(lineRenderer.transform.position);

                }
                else
                {
                    endPos = positionHand;
                }
            }
        }
    }

    private void DrawLine(Vector3 positionHand)
    {
        lineRenderer.positionCount = index1+1;

        Vector3 calculatedPositionLine = CalculatePositionInFrontOfCamera(positionHand);
        lineRenderer.SetPosition(index1, calculatedPositionLine);
        index1++;
        //UpdateCollider();
    }

    public static Vector3 CalculatePositionInFrontOfCamera(Vector3 positionObject)
    {
        return Camera.main.transform.position + positionObject + Camera.main.transform.forward * distance;
    }

    private void UpdateCollider()
    {
        BoxCollider col = myLine.GetComponent<BoxCollider>();
        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(lineLength, 0.1f, 2f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint; // setting position of collider object
        // Following lines calculate the angle between startPos and endPos
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);

    }

    private void addColliderToLine()
    {
        //if (myLine.transform.Find("Collider"))
        //{
        //    Debug.Log("retard");
        //    Destroy(myLine.transform.Find("Collider").gameObject);
        //}
        BoxCollider col = myLine.AddComponent<BoxCollider>();
    }

    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs obj)
    {
        //myLine.transform.position = positionHand;
       
        //vingers in pressed state
        Debug.Log("Source pressed");
    }

    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs obj)
    {
        //vinger is aanwezig in het scherm
        Debug.Log("source detected");
    }

    private void InteractionManager_InteractionSourceReleased(InteractionSourceReleasedEventArgs obj)
    {
        //wanneer je vinger terug loslaat van pinch
        myLine.transform.position = endPos;
        Debug.Log("source released");
    }

    private void OnDestroy()
    {
        InteractionManager.InteractionSourcePressed -= InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
    }
}
