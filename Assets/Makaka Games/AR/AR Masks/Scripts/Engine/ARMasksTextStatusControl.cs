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

using System.Collections;

using UnityEngine;

using TMPro;

[HelpURL("https://makaka.org/unity-assets")]
public class ARMasksTextStatusControl : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public TextMeshProUGUI text;

    public float clearingDelay = 3f;

    private IEnumerator showCoroutineLast;

    public void Show(string status)
    {
        text.text = status;

        if (canvasGroup)
        {
            canvasGroup.alpha = 1f;
        }
    }

    public void Clear()
    {
        text.text = string.Empty;

        if (canvasGroup)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void ShowAndClearWithDelay(string status)
    {
        if (showCoroutineLast != null)
        {
            StopCoroutine(showCoroutineLast);
        }

        showCoroutineLast = ShowCoroutine(status, clearingDelay);

        StartCoroutine(showCoroutineLast);
    }

    private IEnumerator ShowCoroutine(string status, float delay)
    {
        Show(status);

        yield return new WaitForSeconds(delay);

        Clear();
    }
}
