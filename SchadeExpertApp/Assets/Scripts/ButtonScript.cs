using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {
    public Button buttonComponent;
    private ProjectScrollList projectList;


    // Use this for initialization
    void Start () {
        Debug.Log(buttonComponent.name);
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void Setup(ProjectScrollList currentProjectList)
    {
        projectList = currentProjectList;
    }

    public void HandleClick()
    {
        Debug.Log("naam project: " + this.GetComponentInChildren<Text>().text);
        projectList.SetCurrentClickedProject(this.GetComponentInChildren<Text>().text);
        projectList.RemoveProjects();
        projectList.AddFilesFromClickedProjectWrapper();
    }
}
