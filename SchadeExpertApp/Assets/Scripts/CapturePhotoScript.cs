using HoloToolkit.Unity.Buttons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.WebCam;
using System;
using System.IO;
using UnityEngine.EventSystems;
using HoloToolkit.Unity;

public class CapturePhotoScript : MonoBehaviour {
    /// <summary>
    /// Actual camera instance.
    /// </summary>
    private PhotoCapture photoCaptureObject = null;
    private Texture2D targetTexture = null;
    
    public CompoundButton buttonYesPrefab;
    public CompoundButton buttonNoPrefab;

    private CompoundButton buttonYes;

    /// <summary>
    /// The path to the image in the applications local folder.
    /// </summary>
    private string currentImagePath;

    /// <summary>
    /// The path to the users picture folder.
    /// </summary>
    private string pictureFolderPath;


    public void TakePicture()
    {
        Debug.Log("take picture");
        DestroyAllPreviousPhotosInWorld();

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Create a PhotoCapture object
        //eerste argument true als je hologrammen wil zien, anders false
        PhotoCapture.CreateAsync(true, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            //0 betekent geen hologrammen op foto, 1 wel
            cameraParameters.hologramOpacity = 1.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                // Take a picture
                Debug.Log("now for real!");
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });

#if NETFX_CORE
        getPicturesFolderAsync();
#endif
    }


#if NETFX_CORE

    private async void getPicturesFolderAsync() {
        Windows.Storage.StorageLibrary picturesStorage = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        pictureFolderPath = picturesStorage.SaveFolder.Path;
    }

#endif

    private void DestroyAllPreviousPhotosInWorld()
    {
        Debug.Log("destroy all");
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        Debug.Log("begin memorty");
        // Copy the raw image data into our target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // Create a gameobject that we can apply our texture to
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        Debug.Log("before shader");
        quadRenderer.material = new Material(Shader.Find("Standard"));
        Debug.Log("after shader");

        float distance = 2.0f;
        Vector3 CameraPosition = Camera.main.transform.position + Camera.main.transform.forward * distance;

        quad.transform.parent = this.transform;
        //this.transform.position = Camera.main.transform.position;
        quad.transform.position = CameraPosition;

        foreach (Transform child in transform)
        {
            Debug.Log("billboard");
            child.gameObject.AddComponent<Billboard>();
        }


        //var quadHeight = Camera.main.orthographicSize;
        //var quadWidth = quadHeight * Screen.width / Screen.height * 1.0f;
        //quad.transform.localScale = new Vector3(quadWidth, quadHeight, 1);
        //quad.transform.localPosition = Camera.main.transform.position;
        //quad.transform.Translate(0.15f, 0.0f, 0.5f);
        //quad.transform.Translate(1.0f, 0.0f, 0.0f);

        //zet foto op het gameobject
        quadRenderer.material.mainTexture = targetTexture;

        SaveTextureToFile(targetTexture);

        MakeButtonYes(quad);
        MakeButtonNo(quad);

        // Deactivate our camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    public void OnCapturedPhotoToDisk()
    {
        //var cameraRollFolder = Windows.Storage.KnownFolders.CameraRoll.Path; 

#if NETFX_CORE
        try 
        {
            if(pictureFolderPath != null)
            {
                Debug.Log("currentProjectFolder: " + FolderManager.currentProjectFolder.Path);
                System.IO.File.Move(currentImagePath, System.IO.Path.Combine(FolderManager.currentProjectFolder.Path, System.IO.Path.GetFileName(currentImagePath)));
                //System.IO.File.Move(currentImagePath, System.IO.Path.Combine(pictureFolderPath, "Camera Roll", System.IO.Path.GetFileName(currentImagePath)));
            }
        } 
        catch(Exception e) 
        {
                
        }
#else
        Debug.Log("Saved image at " + currentImagePath);
#endif
    }

    private void MakeButtonNo(GameObject quad)
    {
        var buttonNo = Instantiate(buttonNoPrefab, quad.transform.position, buttonNoPrefab.transform.rotation);
        buttonNo.transform.parent = quad.transform;
        buttonNo.transform.localPosition = new Vector3(0.10f, -0.15f, 0);
        //buttonNo.transform.localPosition = buttonYes.transform.localPosition;
        //buttonNo.transform.Translate(0.10f, 0, 0);



        buttonNo.OnButtonPressed += ButtonNo_OnButtonPressed;
    }

    private void MakeButtonYes(GameObject quad)
    {
        buttonYes = Instantiate(buttonYesPrefab, quad.transform.position, buttonYesPrefab.transform.rotation);
        buttonYes.transform.parent = quad.transform;
        buttonYes.transform.localPosition = new Vector3(0, -0.15f, 0);

        //float width = quad.GetComponent<Renderer>().bounds.size.x;
        //float height = quad.GetComponent<Renderer>().bounds.size.y;

        //Vector3 bottom = quad.transform.position;

        //bottom.y -= (height / 3);

        //buttonYes.transform.position = bottom;

        buttonYes.OnButtonPressed += ButtonYes_OnButtonPressed;
    }

    private void ButtonYes_OnButtonPressed(GameObject obj)
    {
        OnCapturedPhotoToDisk();
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }

    private void ButtonNo_OnButtonPressed(GameObject obj)
    {
        File.Delete(currentImagePath);
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }

    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private void SaveTextureToFile(Texture2D texture)
    {
        var bytes = texture.EncodeToPNG();
        string file = string.Format(@"Image_{0:yyyy-MM-dd_hh-mm-ss-tt}.jpg", DateTime.Now);
        currentImagePath = System.IO.Path.Combine(Application.persistentDataPath, file);
        //var path = System.IO.Path.Combine(Application.persistentDataPath, fileName + ".png");
        System.IO.File.WriteAllBytes(currentImagePath, bytes);
    }
}
