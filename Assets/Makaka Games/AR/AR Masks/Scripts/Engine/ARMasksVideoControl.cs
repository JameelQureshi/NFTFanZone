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

#pragma warning disable 649

[HelpURL("https://makaka.org/unity-assets")]
public class ARMasksVideoControl : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    private bool isUsed = false;

    public void PlayFromPauseTime()
    {
        print("videoControl.isUsed: " + isUsed
        + ";\n" + "videoPlayer.isPlaying: " + videoPlayer.isPlaying
        + ";\n" + "videoPlayer.isPaused: " + videoPlayer.isPaused
        + ";\n" + "videoPlayer.time: " + videoPlayer.time
        + ";\n" + "videoPlayer.targetTexture: " + videoPlayer.targetTexture
        );

        double tempTime = videoPlayer.time;

        videoPlayer.Stop();
        videoPlayer.time = tempTime;
        videoPlayer.Play();

        isUsed = true;
    }

    public void Play(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Play();

        isUsed = true;
    }

    public void Stop()
    {
        videoPlayer.Stop();

        isUsed = false;
    }

    /// <summary>
    /// "False" indicates that Video was Paused by this Class
    /// and not by OS automatically
    /// (e.g. Playing Video in Preview Controller).
    /// <para/>
    /// There is no opportunity to catch when Render Texture was stopped by OS.
    /// </summary>
    public bool IsUsed()
    {
        return isUsed && videoPlayer.isPlaying;
    }

    public RenderTexture GetRenderTexture()
    {
        return videoPlayer.targetTexture;
    }

    public void SetRenderTextureWrapModeClamp()
    {
        videoPlayer.targetTexture.wrapMode = TextureWrapMode.Clamp;
    }

    public void SetRenderTextureWrapModeRepeat()
    {
        videoPlayer.targetTexture.wrapMode = TextureWrapMode.Repeat;
    }

    private void OnDestroy()
    {
        SetRenderTextureWrapModeRepeat();
    }
}
