using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorObject : MonoBehaviour, IManipulationHandler, IFocusable
{
    private string AnchorName;
    public bool Undo = false;
    private Vector3 SavedPosition;
    private Quaternion SavedRotation;
    private Vector3 OldPosition;
    private Quaternion OldRotation;

    void Start()
    {
        AnchorName = WorldAnchorManager.GenerateAnchorName(gameObject, GenerateUUID());
        AttachAnchor();
    }

    void Update()
    {
        if (Undo)
        {
            WorldAnchorManager.Instance.RemoveAnchor(this.gameObject);
            RestoreBackupAnchor();
            Undo = false;
        }
    }

    private string GenerateUUID()
    {
        return System.Guid.NewGuid().ToString();
    }

    public void RestoreBackupAnchor()
    {
        Debug.Log("RestoreBackupAnchor");
        this.gameObject.transform.position = OldPosition;
        this.gameObject.transform.rotation = OldRotation;
        WorldAnchorManager.Instance.AttachAnchor(this.gameObject, AnchorName);
    }

    public void OnFocusEnter()
    {
    }

    public void OnFocusExit()
    {
        SavedPosition = this.gameObject.transform.position;
        SavedRotation = this.gameObject.transform.rotation;
    }

    public void AttachAnchor()
    {
        OldPosition = SavedPosition = this.gameObject.transform.position;
        OldRotation = SavedRotation = this.gameObject.transform.rotation;

        WorldAnchorManager.Instance.AttachAnchor(this.gameObject, AnchorName);
        Debug.Log("Anchor attached for: " + this.gameObject.name + " - AnchorID: " + AnchorName);
    }

    public void UpdatePositionAndRemoveAnchor()
    {
        Debug.Log("UpdatePositionAndRemoveAnchor");

        OldPosition = SavedPosition = this.gameObject.transform.position;
        OldRotation = SavedRotation = this.gameObject.transform.rotation;
        WorldAnchorManager.Instance.RemoveAnchor(this.gameObject);
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        //Debug.LogFormat("OnManipulationStarted\r\nSource: {0}  SourceId: {1}\r\nCumulativeDelta: {2} {3} {4}",
        //    eventData.InputSource,
        //    eventData.SourceId,
        //    eventData.CumulativeDelta.x,
        //    eventData.CumulativeDelta.y, 
        //    eventData.CumulativeDelta.z);

        WorldAnchorManager.Instance.RemoveAnchor(this.gameObject);
        Debug.Log("OnManipulationStarted - Anchor Removed");
        OldPosition = SavedPosition;
        OldRotation = SavedRotation;
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        //if (LogGesturesUpdateEvents)
        //{
        //    Debug.LogFormat("OnManipulationUpdated\r\nSource: {0}  SourceId: {1}\r\nCumulativeDelta: {2} {3} {4}",
        //        eventData.InputSource,
        //        eventData.SourceId,
        //        eventData.CumulativeDelta.x,
        //        eventData.CumulativeDelta.y,
        //        eventData.CumulativeDelta.z);
        //}
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        //Debug.LogFormat("OnManipulationCompleted\r\nSource: {0}  SourceId: {1}\r\nCumulativeDelta: {2} {3} {4}",
        //    eventData.InputSource,
        //    eventData.SourceId,
        //    eventData.CumulativeDelta.x,
        //    eventData.CumulativeDelta.y,
        //    eventData.CumulativeDelta.z);

        WorldAnchorManager.Instance.AttachAnchor(this.gameObject, AnchorName);
        Debug.Log("OnManipulationCompleted - Anchor Attached");
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        //    Debug.LogFormat("OnManipulationCanceled\r\nSource: {0}  SourceId: {1}\r\nCumulativeDelta: {2} {3} {4}",
        //        eventData.InputSource,
        //        eventData.SourceId,
        //        eventData.CumulativeDelta.x,
        //        eventData.CumulativeDelta.y,
        //        eventData.CumulativeDelta.z);
    }
}
