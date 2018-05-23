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
    public Transform projectPanel;
    public Transform adjustPanel;
    public Transform deletePanel;

    public SimpleObjectPool projectNameButtonObjectPool;
    public SimpleObjectPool actionButtonObjectPool;


    // Use this for initialization
    void Start () {
        //test();
    }

    public void AddProjectsToScreenWrapper()
    {
        Debug.Log("AddProjectsToScreenWrapper");
        //test();
#if NETFX_CORE
        RemoveItemsFromScreen();
        AddProjectsToScreen();
#endif
    }



#if NETFX_CORE
    private async Task AddProjectsToScreen()
    {
        projectList = await GetAllProjectsAsync();
        var index = 0;
        foreach (StorageFolder project in projectList){
            AddButtonFromObjectpoolToPanel(projectNameButtonObjectPool, projectPanel, project.Name, ButtonTypes.Info, index);

            AddButtonFromObjectpoolToPanel(actionButtonObjectPool, adjustPanel, "A", ButtonTypes.Adjust, index);

            AddButtonFromObjectpoolToPanel(actionButtonObjectPool, deletePanel, "D", ButtonTypes.DeleteProject, index);
            index++;
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
        Debug.Log("current clicked project name:" + currentClickedProject.Name);
#endif
    }

    public void AddFilesFromClickedProjectWrapper()
    {
        Debug.Log("AddFilesFromClickedProjectWrapper");
#if NETFX_CORE
        RemoveItemsFromScreen();
        AddFilesFromClickedProject();
#endif
    }

#if NETFX_CORE
    public async Task AddFilesFromClickedProject()
    {
        currentProjectFiles = await GetAllFilesFromCurrentClickedProjectAsync();
        var index = 0;
        foreach (StorageFile file in currentProjectFiles)
        {
            AddButtonFromObjectpoolToPanel(projectNameButtonObjectPool, projectPanel, file.Name, ButtonTypes.Info, index);

            //AddButtonFromObjectpoolToPanel(actionButtonObjectPool, adjustPanel, file.FileType, ButtonTypes.Info);

            AddButtonFromObjectpoolToPanel(actionButtonObjectPool, deletePanel, "D", ButtonTypes.DeleteFile, index);

            index++;
        }
        SetHeightScrollList(currentProjectFiles.Count);
    }

    public async Task<List<StorageFile>> GetAllFilesFromCurrentClickedProjectAsync()
    {
        Debug.Log("GetAllFilesFromCurrentClickedProjectAsync");
        var tempList = await FolderManager.GetAllFilesFromProject(currentClickedProject);
        Debug.Log("GetAllFilesFromCurrentClickedProjectAsync" + tempList);
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
        RemoveItemsFromPanel(actionButtonObjectPool, adjustPanel);
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
        Debug.Log("index: " + indexToRemove);
#if NETFX_CORE
        projectList.RemoveAt(indexToRemove);
#endif
    }

#if NETFX_CORE
    public async Task DeleteFileFromProjectAsync(int deleteButtonIndex)
    {
        Debug.Log("DeleteFileFromProjectAsync");
        StorageFile fileToDelete = currentProjectFiles[deleteButtonIndex];
        Debug.Log(fileToDelete.Name);
        await fileToDelete.DeleteAsync();
        RemoveItemFromList(deleteButtonIndex);
        AddFilesFromClickedProjectWrapper();
    }

    public async Task DeleteProjectAsync(int deleteButtonIndex)
    {
        Debug.Log("DeleteProjectAsync");
        StorageFolder folderToDelete = projectList[deleteButtonIndex];
        Debug.Log(folderToDelete.Name);
        await folderToDelete.DeleteAsync();
        RemoveItemFromList(deleteButtonIndex);
        AddProjectsToScreenWrapper();
    }
#endif

    public void test()
    {
        foreach (String project in projectList2)
        {
            Debug.Log("foreach addprojects: " + project);

            AddButtonFromObjectpoolToPanel(projectNameButtonObjectPool, projectPanel, project, ButtonTypes.Info, 0);

            AddButtonFromObjectpoolToPanel(actionButtonObjectPool, adjustPanel, "A", ButtonTypes.Adjust, 0);

            AddButtonFromObjectpoolToPanel(actionButtonObjectPool, deletePanel, "D", ButtonTypes.DeleteProject, 0);

        }
        SetHeightScrollList(projectList2.Count);

    }

    private void SetHeightScrollList(int listItemsCount)
    {
        GameObject newButton = projectNameButtonObjectPool.GetObject();        
        contentPanel.GetComponent<LayoutElement>().minHeight = newButton.GetComponent<RectTransform>().sizeDelta.y * listItemsCount;
        projectNameButtonObjectPool.ReturnObject(newButton);
    }
}
