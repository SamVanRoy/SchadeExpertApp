using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if NETFX_CORE
using System.Threading.Tasks;
using Windows.Storage;
#endif


public class ProjectScrollList : MonoBehaviour {
#if NETFX_CORE
    public static List<StorageFolder> projectList;
    public static StorageFolder currentClickedProject;
    public static List<StorageFile> currentProjectFiles;
#endif
    public List<String> projectList2;
    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;
    // Use this for initialization
    void Start () {

    }

    public void AddProjectsToScreenWrapper()
    {
        test();
#if NETFX_CORE
        RemoveProjects();
        AddProjectsToScreen();
#endif
    }



#if NETFX_CORE
    private async Task AddProjectsToScreen()
    {
        var tempList = await FolderManager.GetAllProjectFolders();
        projectList = new List<StorageFolder>(tempList);
        Debug.Log("addProjects: " + projectList.Count);
        foreach (StorageFolder project in projectList){
            Debug.Log("foreach addprojects: " + project.Name);
            GameObject newButton = buttonObjectPool.GetObject();
            //GameObject newButton = GameObject.Instantiate(prefab);
            newButton.transform.SetParent(contentPanel, false);
            newButton.GetComponentInChildren<Text>().text = project.Name;
    
            ButtonScript button = newButton.GetComponent<ButtonScript>();
            button.Setup(this);
        }   
        Debug.Log("childcount3 + " + contentPanel.childCount);

    }
#endif

    public void SetCurrentClickedProject(String clickedProjectName)
    {
#if NETFX_CORE
        currentClickedProject = projectList.Find(project => project.Name == clickedProjectName);
        Debug.Log("current clicked project name:" + currentClickedProject.Name);
#endif
    }

    public void AddFilesFromClickedProjectWrapper()
    {
#if NETFX_CORE
        AddFilesFromClickedProject();
#endif
    }

#if NETFX_CORE
    public async Task AddFilesFromClickedProject()
    {
        var tempList = await FolderManager.GetAllFilesFromProject(currentClickedProject);
        currentProjectFiles = new List<StorageFile>(tempList);
        foreach (StorageFile file in currentProjectFiles)
        {
            Debug.Log("foreach addprojects: " + file.Name);
            GameObject newButton = buttonObjectPool.GetObject();
            //GameObject newButton = GameObject.Instantiate(prefab);
            newButton.transform.SetParent(contentPanel, false);
            newButton.GetComponentInChildren<Text>().text = file.Name;

            ButtonScript button = newButton.GetComponent<ButtonScript>();
            button.Setup(this);
        }
    }
#endif

    public void RemoveProjects()
    {
        while (contentPanel.childCount > 0)
        {
            Debug.Log("childcount1 + " + contentPanel.childCount);
            GameObject toRemove = transform.GetChild(0).gameObject;
            //toRemove.transform.SetParent(null);
            //Destroy(toRemove);
            Debug.Log("while" + toRemove.name);
            buttonObjectPool.ReturnObject(toRemove);
        }
        Debug.Log("childcount2 + " + contentPanel.childCount);

    }

    private void RemoveItem(String itemToRemove)
    {
        for (int i = projectList2.Count - 1; i >= 0; i--)
        {
            if (projectList2[i] == itemToRemove)
            {
                projectList2.RemoveAt(i);
            }
        }
    }

    public void testjeee()
    {        
        Debug.Log(contentPanel.childCount);
        Debug.Log("werkt da nu toch ofwa?" + contentPanel.GetChild(0).name);
    }

    public void test()
    {
        foreach (String project in projectList2)
        {
            Debug.Log("foreach addprojects: " + project);
            GameObject newButton = buttonObjectPool.GetObject();
            //GameObject newButton = GameObject.Instantiate(prefab);
            newButton.transform.SetParent(contentPanel, false);
            newButton.GetComponentInChildren<Text>().text = project;

            ButtonScript button = newButton.GetComponent<ButtonScript>();
            button.Setup(this);
        }
        Debug.Log("childcount2 + " + contentPanel.childCount);

    }
}
