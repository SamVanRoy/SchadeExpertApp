using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if NETFX_CORE
using Windows.Storage;
#endif

public enum ButtonTypes { Adjust, DeleteProject, DeleteFile, Info };

public class ButtonScript : MonoBehaviour {
    public Button buttonComponent;
    private ProjectScrollList projectList;
    public ButtonTypes buttonType;
    public int index;


    // Use this for initialization
    void Start () {
        Debug.Log(buttonComponent.name);
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void SetCurrentProjectlist(ProjectScrollList currentProjectList)
    {
        projectList = currentProjectList;
    }

    public void HandleClick()
    {
        Debug.Log(buttonType);
        switch (buttonType)
        {
            case ButtonTypes.Info:
                AddFilesFromProjectToScreen();
                break;
            case ButtonTypes.Adjust:
                Debug.Log("Adjust");
#if NETFX_CORE
                ShareProjectFilesAsync();
#endif
                break;
            case ButtonTypes.DeleteProject:
                DeleteThisProject();
                break;
            case ButtonTypes.DeleteFile:
                DeleteThisFileAsync();
                break;
        }
        
    }

    private void AddFilesFromProjectToScreen()
    {
        Debug.Log("naam project: " + this.GetComponentInChildren<Text>().text);
        projectList.SetCurrentClickedProject(this.GetComponentInChildren<Text>().text);
        projectList.RemoveItemsFromScreen();
        projectList.AddFilesFromClickedProjectWrapper();
    }


    private async System.Threading.Tasks.Task ShareProjectFilesAsync()
    {
        Debug.Log("ShareProjectFilesAsync");
#if NETFX_CORE
        List<StorageFile> currentProjectFiles = await projectList.GetAllFilesFromCurrentClickedProjectAsync();
        Debug.Log("currentProjectFiles");
        List<String> currentProjectFilesPaths = new List<String>();
        Debug.Log("currentProjectFilesPaths");
        foreach (var file in currentProjectFiles)
        {
            Debug.Log("slet");
            currentProjectFilesPaths.Add(file.Path);
        }
        MailManager.grmbl(currentProjectFilesPaths);
#endif

    }


    private void DeleteThisProject()
    {
#if NETFX_CORE
        projectList.DeleteProjectAsync(index);
#endif
    }

    private void DeleteThisFileAsync()
    {
        Debug.Log("DeleteThisFileAsync");
#if NETFX_CORE
        projectList.DeleteFileFromProjectAsync(index);
#endif
    }

}
