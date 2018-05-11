using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerCommands : MonoBehaviour {
    public void ChangeColorMarker()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }
}
