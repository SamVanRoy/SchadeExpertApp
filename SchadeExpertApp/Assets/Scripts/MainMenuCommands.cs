using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCommands : MonoBehaviour {
    public GameObject markerPrefab;
    private GameObject marker;

    private GameObject mainMenu;

    public void MakeNewMarker()
    {
        marker = Instantiate(markerPrefab, transform.position, markerPrefab.transform.rotation);
        //gameObject.GetComponent<TapToPlace>().IsBeingPlaced
    }

    public void ToggleVisibilityMainMenu(bool visibility)
    {
        if(mainMenu == null)
        {
            mainMenu = GameObject.Find("MainMenu");
        }
        mainMenu.SetActive(visibility);
    }

    public void DrawLine()
    {
        GameObject lineDrawManager = GameObject.Find("LineDrawManager");
        if (lineDrawManager.GetComponent<ReadyGestureDetect>() == null){
            lineDrawManager.AddComponent<ReadyGestureDetect>();
        }
        else
        {
            Destroy(lineDrawManager.GetComponent<ReadyGestureDetect>());
        } 
    }
}
