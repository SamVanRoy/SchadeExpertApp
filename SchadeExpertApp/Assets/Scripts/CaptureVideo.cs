using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using System;
using System.IO;

public class CaptureVideo : MonoBehaviour {

    static readonly float MaxRecordingTime = 2.0f;

    VideoCapture m_VideoCapture = null;
    float m_stopRecordingTimer = float.MaxValue;

    /// <summary>
    /// The path to the users video folder.
    /// </summary>
    private string videoFolderPath;

    private bool isRecording = true;


    void Update()
    {
        if (m_VideoCapture == null || !m_VideoCapture.IsRecording)
        {
            return;
        }

        Debug.Log("Is Recording? : " + isRecording);

        if (!isRecording)
        {
            m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
            Debug.Log("recording stopped!");
        }
    }

    public void StartVideoCaptureTest()
    {

        Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        Debug.Log("resolution: " + cameraResolution);

        float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
        Debug.Log("framerate: " + cameraFramerate);

        VideoCapture.CreateAsync(true, delegate (VideoCapture videoCapture)
        {
            if (videoCapture != null)
            {
                m_VideoCapture = videoCapture;
                Debug.Log("Created VideoCapture Instance!");

                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.frameRate = cameraFramerate;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                m_VideoCapture.StartVideoModeAsync(cameraParameters,
                                                   VideoCapture.AudioState.ApplicationAndMicAudio,
                                                   OnStartedVideoCaptureMode);
            }
            else
            {
                Debug.LogError("Failed to create VideoCapture Instance!");
            }
        });
#if NETFX_CORE
        getVideosFolderAsync();
#endif
    }

#if NETFX_CORE

    private async void getVideosFolderAsync() {
        Windows.Storage.StorageLibrary videosStorage = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        videoFolderPath = videosStorage.SaveFolder.Path;
    }

#endif

    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Video Capture Mode!");
        string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
        string filename = string.Format("TestVideo_{0}.mp4", timeStamp);
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        filepath = filepath.Replace("/", @"\");
#if NETFX_CORE
        if(videoFolderPath != null){
            Debug.Log("filepath1: " + filepath);
            Debug.Log("videoFolderPath1: " + System.IO.Path.Combine(videoFolderPath, "Camera Roll", filename));
            m_VideoCapture.StartRecordingAsync(System.IO.Path.Combine(FolderManager.currentProjectFolder.Path, filename), OnStartedRecordingVideo);
            //m_VideoCapture.StartRecordingAsync(System.IO.Path.Combine(videoFolderPath, "Camera Roll", filename), OnStartedRecordingVideo);
        }
        else{
            Debug.Log("filepath2: " + filepath);
             Debug.Log("videoFolderPath2: " + videoFolderPath);
            m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
        }
#endif
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Video Capture Mode!");
    }

    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");
        isRecording = true;
       // m_stopRecordingTimer = Time.time + MaxRecordingTime;
    }

    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    public void StopRecording()
    {
        Debug.Log("Stoprecording 1: " + isRecording);
        isRecording = false;
        Debug.Log("Stoprecording 2: " + isRecording);
    }
}
