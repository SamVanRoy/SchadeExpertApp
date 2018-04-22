using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
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

    private Vector3 startPos;    // Start position of line
    private Vector3 endPos;    // End position of line


    //Vector3[] positions = new Vector3[3];
    // Use this for initialization
    void Start () {
        //InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        //InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;

        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.material = redSample;
        //lineRenderer.widthMultiplier = 0.01f;

        myLine = new GameObject();
        myLine.AddComponent<LineRenderer>();
        addColliderToLine();
        myLine.AddComponent<TapToPlace>();
        lineRenderer = myLine.GetComponent<LineRenderer>();
        lineRenderer.material = color;
        lineRenderer.widthMultiplier = 0.01f;

        index1 = 0;
        index2 = 0;

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
        //Vector3 heading = Camera.main.transform.forward;
        //heading += new Vector3(0, 0, 2);
        lineRenderer.positionCount = index1+1;
        Debug.Log("index1: " + index1);
        Vector3 adjustedHandposition = new Vector3(positionHand.x, positionHand.y, positionHand.z + 2);
        Debug.Log("adjustedHandposition: " + adjustedHandposition);
        //adjustedHandposition = adjustedHandposition + heading;
        //Debug.Log("adjustedHandposition2: " + adjustedHandposition);
        //myLine.transform.position = heading;
        lineRenderer.SetPosition(index1, adjustedHandposition);
        index1++;
        //UpdateCollider();
    }

    private void UpdateCollider()
    {
        BoxCollider col = myLine.GetComponent<BoxCollider>();
        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(lineLength, 0.1f, 1f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
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
        myLine.transform.position = positionHand;
       
        //draw = true;
        //Vector3 positionHand;
        //obj.state.sourcePose.TryGetPosition(out positionHand);
        ////lineRenderer = GetComponent<LineRenderer>();
        //Debug.Log("Index1: " + index1);
        ////lineRenderer.SetPosition(index1, positionHand);
        //index1++;
        //if (obj.state.anyPressed)
        //{
        //    //obj.state.sourcePose.TryGetPosition(out positionHand);
        //    //positionList.Add(positionHand);
        //    //lineRenderer.SetPositions(positionList);
        //    //lineRenderer.SetPosition(index2, positionHand);
        //    index2++;    
        //}
        //Debug.Log("Index2: " + index2);

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
        Debug.Log("source released");
    }
}
