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

    // Update is called once per frame
    void Update () {
		
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
        Debug.Log("projectname: " + projectName);     
    }

    public void MakeNewProjectFolderWrapper()
    {
#if NETFX_CORE
        MakeNewProjectFolder();
#endif
    }

#if NETFX_CORE
    public async void MakeNewProjectFolder()
    {
        if(projectName != null || projectName.Length > 0) {
            currentProjectFolder = await projectRootFolder.CreateFolderAsync(projectName, CreationCollisionOption.FailIfExists);
            Debug.Log("projectRootFolder: " + projectRootFolder.Name);
            Debug.Log("nieuwe folder: " + currentProjectFolder);
        }
    }
#endif

#if NETFX_CORE
    public static IAsyncOperation<IReadOnlyList<StorageFolder>> GetAllProjectFolders()
    {

        if(Directory.Exists(projectRootFolder.Path)){
            Debug.Log("getAllProjectFolders");
            Debug.Log("projectRootFolder: " + projectRootFolder.Name);
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
        Debug.Log("ontvang event");
#if NETFX_CORE
        StorageFile recordedVoiceFile = await StorageFile.GetFileFromPathAsync(e.RecordingFilePath);
        recordedVoiceFile.CopyAsync(currentProjectFolder, GenerateFileNameWithPrefix("VoiceRecording", "wav"));
#endif
    }

    private static String GenerateFileNameWithPrefix(String filePrefix, String fileExtension)
    {
        return string.Format(@"{0}_{1:yyyy-MM-dd_hh-mm-ss-tt}.{2}", filePrefix, DateTime.Now, fileExtension);
    }

    public void CreateScene()
    {
#if UNITY_EDITOR
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        //SceneManager.CreateScene("testje");
        //SceneManager.LoadScene("testje", LoadSceneMode.Single);
#endif
    }
}
