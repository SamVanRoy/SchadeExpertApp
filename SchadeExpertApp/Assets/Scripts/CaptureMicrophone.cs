using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RecordingFileEventHandler(object sender, RecordingFileEventArgs e);

[RequireComponent(typeof(AudioSource))]
public class CaptureMicrophone : MonoBehaviour {

    public event RecordingFileEventHandler RecordedFile;

    // Copyright (c) Microsoft Corporation. All rights reserved.
    // Licensed under the MIT License. See LICENSE in the project root for license information.

    //NOTE FOR FINAL WORK: Based on MicStreamDemo.cs from Holotoolkit

    /// <summary>
    /// Which type of microphone/quality to access.
    /// </summary>
    public MicStream.StreamCategory StreamType = MicStream.StreamCategory.HIGH_QUALITY_VOICE;

    /// <summary>
    /// Can boost volume here as desired. 1 is default.
    /// <remarks>Can be updated at runtime.</remarks> 
    /// </summary>
    public float InputGain = 1;

    /// <summary>
    /// if keepAllData==false, you'll always get the newest data no matter how long the program hangs for any reason,
    /// but will lose some data if the program does hang.
    /// <remarks>Can only be set on initialization.</remarks>
    /// </summary>
    public bool KeepAllData;

    /// <summary>
    /// If true, Starts the mic stream automatically when this component is enabled.
    /// </summary>
    public bool AutomaticallyStartStream = true;

    /// <summary>
    /// Plays back the microphone audio source though default audio device.
    /// </summary>
    public bool PlaybackMicrophoneAudioSource = true;

    /// <summary>
    /// The name of the file to which to save audio (for commands that save to a file).
    /// </summary>
    public string SaveFileName = "MicrophoneTest.wav";

    /// <summary>
    /// Records estimation of volume from the microphone to affect other elements of the game object.
    /// </summary>
    private float averageAmplitude;

    /// <summary>
    /// Minimum size the demo cube can be during runtime.
    /// </summary>
    [SerializeField]
    private float minObjectScale = .3f;

    private bool isRunning;

    public bool IsRunning
    {
        get { return isRunning; }
        private set
        {
            isRunning = value;
            CheckForErrorOnCall(isRunning ? MicStream.MicPause() : MicStream.MicResume());
        }
    }

    #region Unity Methods

    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        // this is where we call into the DLL and let it fill our audio buffer for us
        CheckForErrorOnCall(MicStream.MicGetFrame(buffer, buffer.Length, numChannels));
    }

    private void OnEnable()
    {
        IsRunning = true;
    }

    private void Awake()
    {
        CheckForErrorOnCall(MicStream.MicInitializeCustomRate((int)StreamType, AudioSettings.outputSampleRate));
        CheckForErrorOnCall(MicStream.MicSetGain(InputGain));

        if (!PlaybackMicrophoneAudioSource)
        {
            gameObject.GetComponent<AudioSource>().volume = 0; // can set to zero to mute mic monitoring
        }

        if (AutomaticallyStartStream)
        {
            CheckForErrorOnCall(MicStream.MicStartStream(KeepAllData, false));
        }

        isRunning = true;

        RecordedFile = new RecordingFileEventHandler(FolderManager.CopyRecordingFileToProject);

    }

    public void StartAudioStreaming()
    {
        Debug.Log("StartAudioStreaming");
        CheckForErrorOnCall(MicStream.MicStartStream(KeepAllData, false));
    }

    public void StopAudioStreaming()
    {
        Debug.Log("StopAudioStreaming");
        CheckForErrorOnCall(MicStream.MicStopStream());
    }

    public void StartRecordingMic()
    {
        Debug.Log("StartRecordingMic");
        CheckForErrorOnCall(MicStream.MicStartRecording(SaveFileName, false));
    }

    public void StopRecordingMic()
    {
        string outputPath = MicStream.MicStopRecording();
        Debug.Log("Saved microphone audio to " + outputPath);

        RecordingFileEventHandler handler = RecordedFile;
        if (handler != null && outputPath != null)
        {
            RecordingFileEventArgs eventArgs = new RecordingFileEventArgs(outputPath);
            // Invokes the delegates.
            Debug.Log("smijt event");
            handler(this, eventArgs);
        }
        CheckForErrorOnCall(MicStream.MicStopStream());
    }

    public void PauseRecording()
    {
        CheckForErrorOnCall(MicStream.MicPause());
    }

    public void UnpauseRecording()
    {
        CheckForErrorOnCall(MicStream.MicResume());
    }

    private void Update()
    {
        CheckForErrorOnCall(MicStream.MicSetGain(InputGain));
    }

    private void OnApplicationPause(bool pause)
    {
        IsRunning = !pause;
    }

    private void OnDisable()
    {
        IsRunning = false;
    }

    private void OnDestroy()
    {
        CheckForErrorOnCall(MicStream.MicDestroy());
    }

#if !UNITY_EDITOR
        private void OnApplicationFocus(bool focused)
        {
            IsRunning = focused;
        }
#endif
    #endregion

    private static void CheckForErrorOnCall(int returnCode)
    {
        MicStream.CheckForErrorOnCall(returnCode);
    }

    public void Enable()
    {
        IsRunning = true;
    }

    public void Disable()
    {
        IsRunning = false;
    }
}



