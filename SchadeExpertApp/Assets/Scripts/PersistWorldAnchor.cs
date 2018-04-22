using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;

public class PersistWorldAnchor : MonoBehaviour {
    public string worldAnchorStoreId;

    WorldAnchorStore anchorStore;
    // Use this for initialization
    void Start()
    {
        WorldAnchorStore.GetAsync(StoreLoaded);

        if (string.IsNullOrEmpty(worldAnchorStoreId))
        {
            worldAnchorStoreId = System.Guid.NewGuid().ToString();
            Debug.Log("New Cube: Name will be;" + worldAnchorStoreId);
        }

    }

    private void StoreLoaded(WorldAnchorStore store)
    {
        this.anchorStore = store;
        // loop through all the anchors and instantiate them
        bool found = false;
        string[] ids = anchorStore.GetAllIds();
        for (int index = 0; index < ids.Length; index++)
        {
            Debug.Log(ids[index]);
            if (ids[index] == worldAnchorStoreId)
            {
                Debug.Log("Found ourselves in store so reloading anchor: " + ids[index]);

                WorldAnchor wa = anchorStore.Load(ids[index], gameObject);
                found = true;
                break;
            }
        }

        if (found == false)
        {
            // Save the game object where it is generated
            WorldAnchor attachingAnchor = gameObject.AddComponent<WorldAnchor>();
            if (attachingAnchor.isLocated)
            {
                // system knows where it is so save it
                Debug.Log("Saving persisted position immediately");
                bool saved = anchorStore.Save(worldAnchorStoreId, attachingAnchor);
                Debug.Log("saved: " + saved);
            }
            else
            {
                Debug.Log("Wating for world anchor");
                attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged; ;
            }
        }
    }

    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            Debug.Log("AttachingAnchor_OnTrackingChanged Saving persisted position in callback:" + worldAnchorStoreId);
            bool saved = anchorStore.Save(worldAnchorStoreId, self);
            Debug.Log("saved: " + saved);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }
}
