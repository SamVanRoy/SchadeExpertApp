using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if NETFX_CORE
using Windows.Storage;
#endif

public enum ButtonTypes { Add, DeleteProject, DeleteFile, Info,  Back};

public class ButtonScript : MonoBehaviour
{
    public Button buttonComponent;
    private ProjectScrollList projectList;
    public ButtonTypes buttonType;
    public int index;

    public GameObject startMenu;
    public GameObject deleteConfirmationScreen;

    public GameObject content;

    public static ButtonTypes lastClickedButtontype;



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
            case ButtonTypes.Add:
                Debug.Log("Add");
                OpenProject();
                break;
            case ButtonTypes.DeleteProject:
                ToggleVisibilityDeleteConfirmationScreen(true);
                lastClickedButtontype = ButtonTypes.DeleteProject;
                break;
            case ButtonTypes.DeleteFile:
                ToggleVisibilityDeleteConfirmationScreen(true);
                lastClickedButtontype = ButtonTypes.DeleteFile;
                break;
            case ButtonTypes.Back:
                projectList.AddProjectsToScreenWrapper();
                break;

        }
        
    }

    private void OpenProject()
    {
        Debug.Log("OpenProject");
        startMenu.GetComponent<StartMenuCommands>().InitWorldNewProject(false);
        if(index == -1)
        {

#if NETFX_CORE
            FolderManager.SetCurrentProjectFolder(ProjectScrollList.currentClickedProject);
#endif
        }
        else
        {
            FolderManager.SetCurrentProjectFolder(index);
        }
    }

    private void AddFilesFromProjectToScreen()
    {
        Debug.Log("naam project: " + this.GetComponentInChildren<Text>().text);
        content.GetComponent<ProjectScrollList>().SetCurrentClickedProject(this.GetComponentInChildren<Text>().text);
        projectList.RemoveItemsFromScreen();
        projectList.AddFilesFromClickedProjectWrapper();
    }

    public void ToggleVisibilityDeleteConfirmationScreen(bool visible)
    {
        deleteConfirmationScreen.SetActive(visible);
    }

    public void DeleteAppropriateButtonType()
    {
        Debug.Log("lastClickedButtontype: " + lastClickedButtontype);
        switch (lastClickedButtontype)
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
