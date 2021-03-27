/*
===================================================================
Unity Assets by MAKAKA GAMES: https://makaka.org/o/all-unity-assets
===================================================================

Online Docs (Latest): https://makaka.org/unity-assets
Offline Docs: You have a PDF file in the package folder.

=======
SUPPORT
=======

First of all, read the docs. If it didn’t help, get the support.

Web: https://makaka.org/support
Email: info@makaka.org

If you find a bug or you can’t use the asset as you need, 
please first send email to info@makaka.org (in English or in Russian) 
before leaving a review to the asset store.

I am here to help you and to improve my products for the best.
*/

using System;

using UnityEngine;
using UnityEngine.UI;

[HelpURL("https://makaka.org/unity-assets")]
public class ARMasksCameraModesControl : MonoBehaviour
{
    public Button buttonStartRecording;

    public Sprite PhotoMode;
    public Sprite VideoMode;

    public Action OnPhotoModeActivated;
    public Action OnVideoModeActivated;

    private Action OnRecordingStarted;

    public Action OnRecordingStartedWithPhotoMode;
    public Action OnRecordingStartedWithVideoMode;

    public void Init (
        Action OnPhotoModeActivated,
        Action OnVideoModeActivated,
        Action OnRecordingStartedWithPhotoMode,
        Action OnRecordingStartedWithVideoMode)
    {
        this.OnPhotoModeActivated = OnPhotoModeActivated;
        this.OnVideoModeActivated = OnVideoModeActivated;

        this.OnRecordingStartedWithPhotoMode = OnRecordingStartedWithPhotoMode;
        this.OnRecordingStartedWithVideoMode = OnRecordingStartedWithVideoMode;

        this.OnRecordingStarted = OnRecordingStartedWithPhotoMode;
    }

    public void ActivatePhotoMode(bool isOn)
    {
        if (isOn)
        {
            print("Photo Mode was Activated.");

            OnPhotoModeActivated?.Invoke();

            buttonStartRecording.image.sprite = PhotoMode;

            OnRecordingStarted = OnRecordingStartedWithPhotoMode;
        }
    }

    public void ActivateVideoMode(bool isOn)
    {
        if (isOn)
        {
            print("Video Mode was Activated.");

            OnVideoModeActivated?.Invoke();

            buttonStartRecording.image.sprite = VideoMode;

            OnRecordingStarted = OnRecordingStartedWithVideoMode;
        }
    }

    public void Record()
    {
        OnRecordingStarted?.Invoke();
    }
}
