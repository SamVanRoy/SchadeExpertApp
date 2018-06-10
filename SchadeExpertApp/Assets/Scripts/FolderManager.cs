using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
#if NETFX_CORE
using Windows.Foundation;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
#endif

public class FolderManager : MonoBehaviour {
    public static string projectName;
    public static string rootFolderName = "Projects";

#if NETFX_CORE
    public static StorageFolder currentProjectFolder;
    public static StorageFolder projectRootFolder;
#endif

    // Use this for initialization
    void Start () {
#if NETFX_CORE
        GetProjectRootFolder();
#endif
    }

#if NETFX_CORE
    public async void GetProjectRootFolder()
    {
        StorageFolder appFolder = ApplicationData.Current.LocalFolder;
        projectRootFolder = await appFolder.GetFolderAsync(rootFolderName);
    }
#endif

    public void GetProjectNameFromInputField(GameObject projectNameInputField)
    {
        projectName = projectNameInputField.GetComponent<InputField>().text;
    }

    public void MakeNewProjectFolderWrapper()
    {
#if NETFX_CORE
        MakeNewProjectFolder();
#endif
    }

    public static void SetCurrentProjectFolder(int currentProjectFolderIndex)
    {
#if NETFX_CORE       
        currentProjectFolder = ProjectScrollList.projectList[currentProjectFolderIndex];
#endif
    }

#if NETFX_CORE       
    public static void SetCurrentProjectFolder(StorageFolder projectFolder)
    {
        currentProjectFolder = projectFolder;
    }
#endif

#if NETFX_CORE
    public async void MakeNewProjectFolder()
    {
        if(projectName != null || projectName.Length > 0) {
            currentProjectFolder = await projectRootFolder.CreateFolderAsync(projectName, CreationCollisionOption.FailIfExists);
        }
    }
#endif

#if NETFX_CORE
    public static IAsyncOperation<IReadOnlyList<StorageFolder>> GetAllProjectFolders()
    {

        if(Directory.Exists(projectRootFolder.Path)){
            return projectRootFolder.GetFoldersAsync();
        }
        else
        {
            Debug.Log("Geen geldig pad getAllProjectFolders");
            return null;
        }
    }
#endif

#if NETFX_CORE
    public static IAsyncOperation<IReadOnlyList<StorageFile>> GetAllFilesFromProject(StorageFolder projectFolder)
    {
        return projectFolder.GetFilesAsync();
    }
#endif

    public static async void CopyRecordingFileToProject(object sender, RecordingFileEventArgs e)
    {
#if NETFX_CORE
        StorageFile recordedVoiceFile = await StorageFile.GetFileFromPathAsync(e.RecordingFilePath);
        recordedVoiceFile.CopyAsync(currentProjectFolder, GenerateFileNameWithPrefix("VoiceRecording", "wav"));
#endif
    }

    private static String GenerateFileNameWithPrefix(String filePrefix, String fileExtension)
    {
        return string.Format(@"{0}_{1:yyyy-MM-dd_hh-mm-ss-tt}.{2}", filePrefix, DateTime.Now, fileExtension);
    }
}
