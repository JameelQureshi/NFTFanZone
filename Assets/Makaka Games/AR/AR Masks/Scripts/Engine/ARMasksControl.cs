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
using System.Collections;

using UnityEngine;
using UnityEngine.XR.ARFoundation;

#if UNITY_IOS

using UnityEngine.Apple.ReplayKit;

#endif

#pragma warning disable 649

[HelpURL("https://makaka.org/unity-assets")]
public class ARMasksControl : MonoBehaviour
{
    public ARFaceManager arFaceManager;

    public ARMasksParametersControl arMasksParametersControl;

    public ARMasksCameraModesControl arMasksCameraModesControl;

    public ARMasksMediaControl arMasksMediaControl;

    [Header("Testing")]

    public GameObject arFaceMeshTest;

    public GameObject canvasTestAnchors;

    [Header("Masks")]

    public ARMaskBaseControl arMaskBaseControl;

    [Header("Texts")]

    public ARMasksTextStatusControl arMasksTextStatusControl;

    public ARMasksTextStatusControl arMasksTextNameControl;

    [Header("Photo/Video")]

    public int sizeMaxOfImagePicked = 1024;

    public Animator animatorCameraShutter;

    public GameObject canvasesToHideForRecording;
    public GameObject canvasToStopVideoRecording;

    private bool canVideoPreviewBeShown = false;
    private bool isVideoPreviewShowed = false;
    private bool wasVideoRecordingStarted = false;

    [SerializeField]
    private ARMasksVideoControl arMasksVideoControl;

    private void Awake()
    {
        arMasksTextStatusControl.Clear();

#if !UNITY_EDITOR

        arFaceMeshTest.SetActive(false);

        canvasTestAnchors.SetActive(false);
#endif

    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) // Permission Window of Apple ReplayKit was closed
        {
            StopVideoRecordingOnDontAllow();
        }
    }

    private void Update()
    {
        PlayVideoFromPauseTimeAfterJustRecordedVideoPreviewWasClosed();

        ShowVideoPreviewAfterStopRecording();
    }

    private IEnumerator Start()
    {
        arMasksTextStatusControl.Clear();

        arMaskBaseControl.Init();

        yield return StartCoroutine(
            arMasksParametersControl.InitCoroutine(arMaskBaseControl.GetIcons()));

        arMaskBaseControl.OnParametersReset =
        () =>
        {
            arMasksParametersControl.ActivateParameter(0, false);
        };

        arMaskBaseControl.OnStatusSet =
        (status) =>
        {
            arMasksTextStatusControl.ShowAndClearWithDelay(status);
        };

        // Prepare Actions for All AR Faces in Frame
        // if AR Mask operates with their Materials

        for (int i = 0; i < arMaskBaseControl.GetParametersCount(); i++)
        {
            int indexForLambda = i;

            arMaskBaseControl.GetParameter(indexForLambda).OnParameterValueChanged =
                (value) =>
                {
                    ChangeMaterialForAllARFaces(value, indexForLambda);
                };
        }

        // Link Toggling with Parameters & Slider

        arMasksParametersControl.OnParameterActivating =
            (index) =>
            {
                return
                    arMaskBaseControl.ActivateParameterAndGetNormilized(index);
            };

        // Link Slider Value Changing & Parameters

        arMasksParametersControl.OnToggleSecondClick =
        (index) =>
        {
            arMaskBaseControl.ResetParameterWithoutClickingOnSlider(index);
        };

        arMasksParametersControl.OnSetParameterValue =
            (value) =>
            {
                arMaskBaseControl.SetActivatedParameterFromNormalized(value);
            };

        arMaskBaseControl.OnSetParameterValueWithoutClickingOnSlider =
            (index, value) =>
            {
                arMasksParametersControl.SetSliderValueWithoutNotify(index, value);
            };

        //After Whole Linking

        //Connect Default Image Texture with Wrap Mode Changing

        arMaskBaseControl.ResetTexture(
            arFaceManager.facePrefab
                .GetComponent<MeshRenderer>().sharedMaterial.mainTexture);

        arMaskBaseControl.ResetParametersCompletely();

        // Show Status

        arMasksTextNameControl.ShowAndClearWithDelay(
            arMaskBaseControl.GetName());

        //Init Camera Modes

        arMasksCameraModesControl.Init(
            arMasksTextStatusControl.Clear,
            () =>
            {
                arMasksTextStatusControl.ShowAndClearWithDelay("Tap to Stop");

#if UNITY_IOS

                //Activate ReplayKit to avoid Error Message

                print(
                    "ReplayKit Activating (false is normal here for supported device): "
                    + ReplayKit.APIAvailable);

#endif

            },
            TakePhotoAndSave,
            StartVideoRecording);

        arMasksMediaControl.OnImagePicked = OnImagePicked;
        arMasksMediaControl.OnVideoPicked = OnVideoPicked;
    }

    public void PickImage()
    {
        arMasksMediaControl.PickImageSafely(sizeMaxOfImagePicked);
    }

    private void OnImagePicked(Texture2D texture)
    {
        if (texture == null)
        {
            arMasksTextStatusControl.ShowAndClearWithDelay("Couldn't load texture from this path");
        }
        else
        {
            if (arFaceManager == null && arFaceManager.subsystem == null)
            {
                arMasksTextStatusControl.ShowAndClearWithDelay("arFaceManager = "
                    + arFaceManager + " ; subsystem = " + arFaceManager.subsystem);
            }
            else
            {
                if (arFaceManager.trackables.count > 0 || Application.isEditor)
                {
                    ChangeMaterialForAllARFaces((material) =>
                    {
                        material.mainTexture = texture;
                    });

                    arMasksVideoControl.Stop();

                    arMaskBaseControl.ResetTexture(texture);
                    arMaskBaseControl.ResetParametersCompletely();
                }
                else
                {
                    arMasksTextStatusControl.ShowAndClearWithDelay(
                        "No Faces in Frame");
                }
            }
        }
    }

    public void PickVideo()
    {
        arMasksMediaControl.PickVideoSafely();
    }

    private void OnVideoPicked(string path, float rotation)
    {
        if (string.IsNullOrEmpty(path))
        {
            if (arMasksVideoControl.IsUsed())
            {
                // iOS: because Current Video on Mask
                // can be paused by OS automatically
                // when it was played in Picker Menu

                arMasksVideoControl.PlayFromPauseTime();
            }
        }
        else
        {
            if (arFaceManager == null && arFaceManager.subsystem == null)
            {
                arMasksTextStatusControl.ShowAndClearWithDelay("arFaceManager = " + arFaceManager
                    + " ; subsystem = " + arFaceManager.subsystem);
            }
            else
            {
                if (arFaceManager.trackables.count > 0 || Application.isEditor)
                {
                    ChangeMaterialForAllARFaces((material) =>
                    {
                        material.mainTexture =
                            arMasksVideoControl.GetRenderTexture();
                    });

                    arMasksVideoControl.Play("file://" + path);

                    arMaskBaseControl.ResetTexture(
                        arMasksVideoControl.GetRenderTexture());

                    arMaskBaseControl.ResetParametersCompletely();

                    if (rotation == 90f)
                    {
                        rotation = -Mathf.PI / 2f;
                    }
                    else if (rotation == 180f)
                    {
                        rotation = Mathf.PI;
                    }

                    arMaskBaseControl.SetParameterWithoutClickingOnSliderByShaderParameterName(
                        ARMaskBaseParameterControl.SHADER_PARAMETER_NAME_ROTATION,
                        rotation,
                        true);
                }
                else
                {
                    arMasksTextStatusControl.ShowAndClearWithDelay(
                        "No Faces in Frame");
                }
            }
        }
    }

    public void TakePhotoAndSave()
    {
        StartCoroutine(TakePhotoAndSaveCoroutine());
    }

    private IEnumerator TakePhotoAndSaveCoroutine()
    {
        canvasesToHideForRecording.SetActive(false);

        yield return new WaitForEndOfFrame();

        Texture2D screenshot = new Texture2D (
            Screen.width, Screen.height, TextureFormat.RGB24, false);

        screenshot.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        yield return new WaitForEndOfFrame();

        canvasesToHideForRecording.SetActive(true);

        yield return new WaitForEndOfFrame();

        //arMasksMediaControl.SaveImageToGallery(
        //    screenshot,
        //    "AR Masks",
        //    "Image.png",
        //    arMasksTextStatusControl.ShowAndClearWithDelay);
        
        // Memory Control

        Destroy(screenshot);

        // Run Shutter

        animatorCameraShutter.SetTrigger("Take Photo");
    }

    //Run On Button Click
    public void StartVideoRecording()
    {

#if UNITY_IOS

        if (ReplayKit.APIAvailable)
        {
            try
            {
                if (!ReplayKit.isRecording)
                {
                    ReplayKit.StartRecording(true);

                    wasVideoRecordingStarted = true;

                    canvasesToHideForRecording.SetActive(false);

                    canvasToStopVideoRecording.SetActive(true);
                }
            }
            catch (Exception e)
            {
                arMasksTextStatusControl.ShowAndClearWithDelay(e.ToString());

                print("StartVideoRecording() error:" + e.ToString());
            }
        }
        else
        {
            arMasksTextStatusControl.ShowAndClearWithDelay(
                "ReplayKit is not available."
                + " Requirements: "
                + ARFoundationSupportChecker.GetRequirementsForFaceTracking()
                + ".");
        }

#elif UNITY_ANDROID

        //arMasksTextStatusControl.ShowWithClearing(
        //        "Implement \"StartVideoRecording\" on Android here");

#endif

    }

    private void StopVideoRecordingBase()
    {
        print("StopVideoRecordingBase() Call");

        canvasesToHideForRecording.SetActive(true);

        canvasToStopVideoRecording.SetActive(false);

        wasVideoRecordingStarted = false;
    }

    public void StopVideoRecording()
    {
        StopVideoRecordingBase();

#if UNITY_IOS

        if (ReplayKit.APIAvailable)
        {
            try
            {
                if (ReplayKit.isRecording)
                {
                    ReplayKit.StopRecording();

                    arMasksTextStatusControl.Show("Video Creating...");

                    canVideoPreviewBeShown = true;
                }
            }
            catch (Exception e)
            {
                arMasksTextStatusControl.ShowAndClearWithDelay(e.ToString());

                print("StopVideoRecording() error:" + e.ToString());
            }
        }
        else
        {
            arMasksTextStatusControl.ShowAndClearWithDelay(
                "ReplayKit is not available."
                + " Requirements: "
                + ARFoundationSupportChecker.GetRequirementsForFaceTracking()
                + ".");
        }

#elif UNITY_ANDROID

        //arMasksTextStatusControl.ShowWithClearing(
        //        "Implement \"StopVideoRecording\" on Android here");

#endif

    }

    private void StopVideoRecordingOnDontAllow()
    {

#if UNITY_IOS

        if (wasVideoRecordingStarted && ReplayKit.APIAvailable)
        {
            try
            {
                if (!ReplayKit.isRecording)
                {
                    StopVideoRecordingBase();
                }
            }
            catch (Exception e)
            {
                print("StopVideoRecordingOnDontAllow() error:" + e.ToString());
            }
        }

#endif

    }

    private void ShowVideoPreviewAfterStopRecording()
    {

#if UNITY_IOS

        if (canVideoPreviewBeShown
            && ReplayKit.APIAvailable
            && ReplayKit.recordingAvailable)
        {
            canVideoPreviewBeShown = false;

            ReplayKit.Preview();

            arMasksTextStatusControl.Clear();

            isVideoPreviewShowed = true;
        }

#endif

    }

    private void PlayVideoFromPauseTimeAfterJustRecordedVideoPreviewWasClosed()
    {

#if UNITY_IOS

        if (arMasksVideoControl.IsUsed()
            && isVideoPreviewShowed
            && !ReplayKit.isPreviewControllerActive)
        {
            isVideoPreviewShowed = false;

            // iOS: because Current Video on Mask
            // can be paused by OS automatically
            // when video was played in Preview Controller

            arMasksVideoControl.PlayFromPauseTime();
        }

#endif

    }


    public void TapScreen()
    {
        //print("TapScreen()");

        arMaskBaseControl.OnScreenTap?.Invoke();
    }


    public void ChangeMaterialForAllARFaces(float value, int indexParameter)
    {
        ChangeMaterialForAllARFaces((material) =>
        {
            arMaskBaseControl.GetParameter(indexParameter).
                OnMaterialChanged?.Invoke(value, material, indexParameter);
        });
    }

    public void ChangeMaterialForAllARFaces(Action<Material> callback)
    {

#if UNITY_EDITOR

        callback(arFaceMeshTest.GetComponent<MeshRenderer>().material);

#else
        foreach (ARFace arFace in arFaceManager.trackables)
        {
            Material material = arFace.GetComponent<MeshRenderer>().material;

            if (material && callback != null)
            {
                callback(material);
            }
        }
#endif

    }

}
