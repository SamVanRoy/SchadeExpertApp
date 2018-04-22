using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCommands : MonoBehaviour {
    public GameObject markerPrefab;
    private GameObject marker;

    private GameObject mainMenu;
    public GameObject lineDrawer;

    public void MakeNewMarker()
    {

        //prefabje.transform.Rotate(180f, 0, 0);
        marker = Instantiate(markerPrefab, transform.position, markerPrefab.transform.rotation);
        marker.AddComponent<AnchorObject>();
        //marker.AddComponent<PersistWorldAnchor>();
        //marker.gameObject.tag = "Marker";

        //Instantiate(Marker, new Vector3(0,0,0), prefabje.transform.rotation);
    }

    public void ChangeColorMarker()
    {
        //hier iets doen in de genre van het oproepen van marker waar je naar gazed of waarvan je de menu neemt
        marker.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.red);
    }

    public void ToggleMenu(bool visibility)
    {
        if(mainMenu == null)
        {
            mainMenu = GameObject.Find("MainMenu");
        }
        mainMenu.SetActive(visibility);
    }

    //public void GoBackToStartMenu()
    //{
    //    SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    //}

    public void ToggleLineDrawerScript()
    {
        //lineDrawer.AddComponent<ReadyGestureDetect>();
        lineDrawer.GetComponent<ReadyGestureDetect>().enabled = !lineDrawer.GetComponent<ReadyGestureDetect>().isActiveAndEnabled;
    }

    public void testeke()
    {
        GameObject lineDrawManager = GameObject.Find("LineDrawManager");
        if (lineDrawer.GetComponent<ReadyGestureDetect>() == null){
            Debug.Log("add script");
            lineDrawManager.AddComponent<ReadyGestureDetect>();
        }
        else
        {
            Debug.Log("DESTROY script");
            Destroy(lineDrawer.GetComponent<ReadyGestureDetect>());
        }
    }
}
