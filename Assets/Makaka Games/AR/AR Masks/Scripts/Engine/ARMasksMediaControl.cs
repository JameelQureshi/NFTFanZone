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

using UnityEngine;
using UnityEngine.Video;

#if UNITY_EDITOR

using UnityEditor;

#endif

using System;
using System.IO;

#pragma warning disable 649

[HelpURL("https://makaka.org/unity-assets")]
public class ARMasksMediaControl : MonoBehaviour
{
    public Action<Texture2D> OnImagePicked;

    public Action<string, float> OnVideoPicked;

#if UNITY_EDITOR

    [Header("Image")]

    [SerializeField]
    private Texture[] imagesForEditorTesting;
    private int indexImageForEditorTesting;

    [Header("Video")]

    [SerializeField]
    private float videoRotationTestInEditor = 90f;

    [SerializeField]
    private VideoClip[] videosForEditorTesting;
    private int indexVideoForEditorTesting;

#endif

    public void PickImageSafely(int maxSize)
    {
       
    }

    /// <summary>
    /// Pick a PNG image from Gallery/Photos
    /// If the selected image's width and/or height is greater than maxSize,
    /// then down-scale the image.
    /// </summary>
    private void PickImage(int maxSize)
    {
      
            
    }

    /// <summary>
    /// Save the screenshot to Gallery (Android) / Photos (iOS).
    /// </summary>
    //public void SaveImageToGallery(
    //    Texture2D image,
    //    string album,
    //    string filename,
    //    NativeGallery.MediaSaveCallback callbackError = null)
    //{
    //    print("Permission result: " + NativeGallery.SaveImageToGallery(
    //        image, album, filename, callbackError).ToString());
    //}

    public void PickVideoSafely()
    {
        
    }

    private void PickVideo()
    {
        

    }
}
