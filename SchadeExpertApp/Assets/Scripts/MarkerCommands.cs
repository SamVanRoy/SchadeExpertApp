using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerCommands : MonoBehaviour {
    public void ChangeColorMarker(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }
}
