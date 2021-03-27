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
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// There is a checking for AR support before the ARSession is enabled.
/// For ARCore in particular, it is possible for a device to support ARCore but not
/// have it installed. This example will detect this case and prompt the user to install ARCore.
/// To test this feature yourself, use a supported device and uninstall ARCore.
/// (Settings > Search for "ARCore" and uninstall or disable it.)
/// </summary>

[HelpURL("https://makaka.org/unity-assets")]
public class ARFoundationSupportChecker : MonoBehaviour
{
    [Tooltip("Is AR Foundation Support checked in Editor to Test initial UI without Building?")]
    public bool isCheckedInEditorOnInit = false;

    public ARSession arSession;

    private const string RequiremetnsFaceTrackingIOS =
        "iOS 11.0+, iPhone X+, iPad Pro (11-inch or 12.9-inch 3G)";

    private const string RequiremetnsFaceTrackingAndroid =
        "Google: ARCore supported devices.";

    [Space]
    [Header("Cross-Platform Events")]

    public UnityEvent OnARReady;

    // String - Status Text
    public UnityEventString OnARUnsupported;

    [Space]
    [Header("Android Only Events")]

    public GameObject buttonInstall;

    [Space]

    // String - Status Text
    public UnityEventString OnARSoftwareUpdateFailed;

    [Serializable]
    public class UnityEventString : UnityEvent<string> { }

    private void Start()
    {

#if UNITY_EDITOR

        if (isCheckedInEditorOnInit)
        {
            Check();
        }
        else
        {
            SuccessTest();

            OnARReady?.Invoke();
        }

#else

        Check();

#endif

    }

    private void Check()
    {
        StartCoroutine(CheckCoroutine());
    }

    private IEnumerator CheckCoroutine()
    {
        SetButtonInstallActive(false);

        print("Checking for AR Foundation support...");

        yield return ARSession.CheckAvailability();

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            print("Your device supports AR Foundation, but requires a software update.");
            print("Attempting install...");

            yield return ARSession.Install();
        }

        if (ARSession.state == ARSessionState.Ready)
        {
            print("Your device supports AR Foundation!");
            print("Starting AR session...");

            Success();
        }
        else
        {
            switch (ARSession.state)
            {
                case ARSessionState.Unsupported:

                    print(
                        "AR Foundation is Not Supported on Your Device. Requirements: "
                        + GetRequirementsForFaceTracking()
                        + ".");

                    OnARUnsupported?.Invoke("AR is Not Supported. Requirements: "
                        + GetRequirementsForFaceTracking()
                        + ".");

                    break;

                case ARSessionState.NeedsInstall:

                    FailInstall();

                    break;
            }
        }
    }

    public static string GetRequirementsForFaceTracking()
    {

#if UNITY_IOS

        return RequiremetnsFaceTrackingIOS;

#elif UNITY_ANDROID

        return RequiremetnsFaceTrackingAndroid;

#endif

    }

    private void Success()
    {
        arSession.enabled = true;

        OnARReady?.Invoke();
    }

    public void SuccessTest()
    {
        print("AR Foundation is Supported: Success Test in Editor."
            + " Real Testing is only on Real Device.");

        Success();

        SetButtonInstallActive(false);
    }

    private void FailInstall()
    {
        print("The software update failed, or you declined the update.");

        OnARSoftwareUpdateFailed?.Invoke(
            "The software update failed, or you declined the update.");

        // In this case, we enable a button which allows the user
        // to try again in the event they decline the update the first time.

        SetButtonInstallActive(true);
    }

    private void SetButtonInstallActive(bool active)
    {
        if (buttonInstall != null)
        {
            buttonInstall.SetActive(active);
        }
    }

    public void OnInstallButtonPressed()
    {
        StartCoroutine(InstallARSoftware());
    }

    private IEnumerator InstallARSoftware()
    {
        SetButtonInstallActive(false);

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            print("Attempting install...");

            yield return ARSession.Install();

            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                FailInstall();
            }
            else if (ARSession.state == ARSessionState.Ready)
            {
                Success();
            }
        }
        else
        {
            print("Error: AR Software does not require install.");
        }
    }
}
