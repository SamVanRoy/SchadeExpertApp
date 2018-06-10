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
    public Transform contentPanel;
    public Transform projectPanel;
    public Transform addPanel;
    public Transform deletePanel;
    public GameObject headerPanel;

    public SimpleObjectPool projectNameButtonObjectPool;
    public SimpleObjectPool actionButtonObjectPool;

    public GameObject emptyListLabel;


    // Use this for initialization
    void Start () {
    }

    public void AddProjectsToScreenWrapper()
    {
#if NETFX_CORE
        RemoveItemsFromScreen();
        AddProjectsToScreen();
#endif
    }



#if NETFX_CORE
    private async Task AddProjectsToScreen()
    {
        headerPanel.SetActive(false);

        projectList = await GetAllProjectsAsync();
        if(projectList.Count == 0)
        {
            emptyListLabel.SetActive(true);
            emptyListLabel.GetComponent<TextMesh>().text = "Geen projecten aanwezig";
        }
        else{
            emptyListLabel.SetActive(false);

            var index = 0;
            foreach (StorageFolder project in projectList){
                AddButtonFromObjectpoolToPanel(projectNameButtonObjectPool, projectPanel, project.Name, ButtonTypes.Info, index);

                AddButtonFromObjectpoolToPanel(actionButtonObjectPool, addPanel, "A", ButtonTypes.Add, index);

                AddButtonFromObjectpoolToPanel(actionButtonObjectPool, deletePanel, "D", ButtonTypes.DeleteProject, index);
                index++;
            }
        }        
        SetHeightScrollList(projectList.Count);
    }

    private async Task<List<StorageFolder>> GetAllProjectsAsync()
    {
        return new List<StorageFolder>(await FolderManager.GetAllProjectFolders());
    }
#endif

    public void SetCurrentClickedProject(String clickedProjectName)
    {
#if NETFX_CORE
        currentClickedProject = projectList.Find(project => project.Name == clickedProjectName);
#endif
    }

    public void AddFilesFromClickedProjectWrapper()
    {
#if NETFX_CORE
        RemoveItemsFromScreen();
        AddFilesFromClickedProject();
#endif
    }

#if NETFX_CORE
    public async Task AddFilesFromClickedProject()
    {
        headerPanel.SetActive(true);
        currentProjectFiles = await GetAllFilesFromCurrentClickedProjectAsync();
        if(currentProjectFiles.Count == 0)
        {
            emptyListLabel.SetActive(true);
            emptyListLabel.GetComponent<TextMesh>().text = "Geen files aanwezig";
        }
        else{
            emptyListLabel.SetActive(false);

            var index = 0;
            foreach (StorageFile file in currentProjectFiles)
            {
                AddButtonFromObjectpoolToPanel(projectNameButtonObjectPool, projectPanel, file.Name, ButtonTypes.Info, index);

                AddButtonFromObjectpoolToPanel(actionButtonObjectPool, deletePanel, "D", ButtonTypes.DeleteFile, index);

                index++;
            }
        }
        SetHeightScrollList(currentProjectFiles.Count);
    }

    public async Task<List<StorageFile>> GetAllFilesFromCurrentClickedProjectAsync()
    {
        var tempList = await FolderManager.GetAllFilesFromProject(currentClickedProject);
        return new List<StorageFile>(tempList);
    }
#endif

    private GameObject AddButtonFromObjectpoolToPanel(SimpleObjectPool objectPool, Transform panel, String text, ButtonTypes buttonType, int index)
    {
        GameObject newButton = objectPool.GetObject();
        newButton.transform.SetParent(panel, false);
        newButton.GetComponentInChildren<Text>().text = text;

        ButtonScript buttonScript = newButton.GetComponent<ButtonScript>();
        buttonScript.buttonType = buttonType;
        buttonScript.index = index;

        return newButton;
    }

    public void RemoveItemsFromScreen()
    {
        RemoveItemsFromPanel(projectNameButtonObjectPool, projectPanel);
        RemoveItemsFromPanel(actionButtonObjectPool, addPanel);
        RemoveItemsFromPanel(actionButtonObjectPool, deletePanel);
    }

    private void RemoveItemsFromPanel(SimpleObjectPool objectPool, Transform panel)
    {
        while (panel.childCount > 0)
        {
            objectPool.ReturnObject(panel.GetChild(0).gameObject);
        }
    }

    public void RemoveItemFromList(int indexToRemove)
    {
#if NETFX_CORE
        projectList.RemoveAt(indexToRemove);
#endif
    }

#if NETFX_CORE
    public async Task DeleteFileFromProjectAsync(int deleteButtonIndex)
    {
        StorageFile fileToDelete = currentProjectFiles[deleteButtonIndex];
        await fileToDelete.DeleteAsync();
        RemoveItemFromList(deleteButtonIndex);
        AddFilesFromClickedProjectWrapper();
    }

    public async Task DeleteProjectAsync(int deleteButtonIndex)
    {
        StorageFolder folderToDelete = projectList[deleteButtonIndex];
        await folderToDelete.DeleteAsync();
        RemoveItemFromList(deleteButtonIndex);
        AddProjectsToScreenWrapper();
    }
#endif

    private void SetHeightScrollList(int listItemsCount)
    {
        GameObject newButton = projectNameButtonObjectPool.GetObject();        
        contentPanel.GetComponent<LayoutElement>().minHeight = (newButton.GetComponent<RectTransform>().sizeDelta.y * listItemsCount) + 30;
        projectNameButtonObjectPool.ReturnObject(newButton);
    }
}
