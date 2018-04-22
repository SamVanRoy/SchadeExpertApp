using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

#if NETFX_CORE
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class InitProjectRootFolder : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if NETFX_CORE
        StorageFolder appFolder = ApplicationData.Current.LocalFolder;
        appFolder.CreateFolderAsync(FolderManager.rootFolderName, CreationCollisionOption.FailIfExists);      
#endif

    }

    /*
     * #if NETFX_CORE
        if (Directory.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path, "projects"))
        {
            Debug.Log("geldig pad");
        }
        else
        {
            Directory.CreateDirectory(Path.Combine(ApplicationData.Current.LocalFolder.Path, "projects"));
            Debug.Log("Geen geldig pad");
        }
#endif
        **/
}
