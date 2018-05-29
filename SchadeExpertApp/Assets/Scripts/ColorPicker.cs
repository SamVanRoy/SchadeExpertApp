using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

//based on gazeablecolorpicker script from holotoolkit.

public class ColorPicker : MonoBehaviour, IFocusable, IInputClickHandler
{

    public Renderer rendererComponent;

    [System.Serializable]
    public class PickedColorCallback : UnityEvent<Color> { }

    public PickedColorCallback OnGazedColor = new PickedColorCallback();
    public PickedColorCallback OnPickedColor = new PickedColorCallback();

    private bool gazing = false;

    private GameObject colorPickerScreen;
    private GameObject objectToColor;


    private void Update()
    {
        if (gazing == false) return;
        UpdatePickedColor(OnGazedColor);
    }

    private void UpdatePickedColor(PickedColorCallback cb)
    {
        RaycastHit hit = GazeManager.Instance.HitInfo;

        if (hit.transform.gameObject != rendererComponent.gameObject) { return; }

        var texture = (Texture2D)rendererComponent.material.mainTexture;

        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= texture.width;
        pixelUV.y *= texture.height;

        Color col = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
        ChangeColorObject(col);
        cb.Invoke(col);
    }

    public void OnFocusEnter()
    {
        gazing = true;
    }

    public void OnFocusExit()
    {
        gazing = false;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        UpdatePickedColor(OnPickedColor);
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }

    public void InitializeColorPicker(GameObject objectToColor, GameObject colorPickerScreen)
    {
        this.colorPickerScreen = colorPickerScreen;
        this.objectToColor = objectToColor;
    }

    public void ChangeColorObject(Color color)
    {
        objectToColor.GetComponent<Renderer>().material.color = color;
    }
}
