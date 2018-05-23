using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if NETFX_CORE
using Windows.Storage;
#endif

public enum ButtonTypes { Adjust, DeleteProject, DeleteFile, Info };

public class ButtonScript : MonoBehaviour
{
    public Button buttonComponent;
    private ProjectScrollList projectList;
    public ButtonTypes buttonType;
    public int index;

    public GameObject startMenu;
    public GameObject deleteConfirmationScreen;

    public GameObject content;



    // Use this for initialization
    void Start () {
        Debug.Log(buttonComponent.name);
        buttonComponent.onClick.AddListener(HandleClick);
        projectList = content.GetComponent<ProjectScrollList>();
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
                OpenProject();
//#if NETFX_CORE
//                ShareProjectFilesAsync();
//#endif
                break;
            case ButtonTypes.DeleteProject:
                ToggleVisibilityDeleteConfirmationScreen(true);
                break;
            case ButtonTypes.DeleteFile:
                ToggleVisibilityDeleteConfirmationScreen(true);
                DeleteThisFileAsync();
                break;
        }
        
    }

    private void OpenProject()
    {
        Debug.Log("OpenProject");
        startMenu.GetComponent<StartMenuCommands>().InitWorldNewProject(false);
        FolderManager.SetCurrentProjectFolder(index);
    }

    private void AddFilesFromProjectToScreen()
    {
        Debug.Log("naam project: " + this.GetComponentInChildren<Text>().text);
        content.GetComponent<ProjectScrollList>().SetCurrentClickedProject(this.GetComponentInChildren<Text>().text);
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
        //MailManager.feestje();
#endif

    }

    public void ToggleVisibilityDeleteConfirmationScreen(bool visible)
    {
        deleteConfirmationScreen.SetActive(visible);
    }

    public void DeleteAppropriateButtonType()
    {
        switch (buttonType)
        {
            case ButtonTypes.DeleteProject:
                DeleteThisProject();
                break;
            case ButtonTypes.DeleteFile:
                DeleteThisFileAsync();
                break;
        }
    }

    public void DeleteThisProject()
    {
        ToggleVisibilityDeleteConfirmationScreen(false);
        Debug.Log("DeleteThisProject");
        Debug.Log("Sampie2: " + projectList.name);

#if NETFX_CORE
        Debug.Log("DeleteThisProject2");
        projectList.DeleteProjectAsync(index);
        Debug.Log("DeleteThisProject3");
#endif
    }

    private void DeleteThisFileAsync()
    {
        Debug.Log("DeleteThisFileAsync");
        ToggleVisibilityDeleteConfirmationScreen(false);
#if NETFX_CORE
        projectList.DeleteFileFromProjectAsync(index);
#endif
    }
}
