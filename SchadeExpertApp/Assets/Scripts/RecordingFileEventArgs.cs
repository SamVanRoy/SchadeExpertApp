using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingFileEventArgs : EventArgs
{
    private string recordingFilePath;
    public RecordingFileEventArgs(string recordingFilePath)
    {
        this.recordingFilePath = recordingFilePath;
    }

    public string RecordingFilePath
    {
        get { return recordingFilePath; }
    }
}
