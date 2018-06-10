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
        MainMenuCommands.colorPickerScreen.SetActive(false);
    }

    public void InitializeColorPicker(GameObject objectToColor)
    {
        this.objectToColor = objectToColor;
    }

    public void ChangeColorObject(Color color)
    {
        if(objectToColor != null)
        {
            objectToColor.GetComponent<Renderer>().material.color = color;
        }
        else
        {
            MainMenuCommands.colorPickerScreen.SetActive(false);
        }
    }
}
