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
using System.Linq;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

#pragma warning disable 649

[HelpURL("https://makaka.org/unity-assets")]
public class ARMasksParametersControl : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup toggleGroup;

    private Toggle[] toggles;

    private Toggle toggleActiveTemp;

    /// <summary>
    /// Set Only in <see cref="ActivateParameter"/>
    /// </summary>
    private int toggleActivatedIndex = -1;


    [SerializeField]
    private Slider sliderActiveValue;

    [SerializeField]
    private TextMeshProUGUI textActiveValue;


    public Func<int, float> OnParameterActivating;

    public Action<float> OnSetParameterValue;

    public Action<int> OnToggleSecondClick;

    [Space]
    [Header("Scrolling")]

    [SerializeField]
    private RectTransform canvas;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField]
    private RectTransform parameterButtonTemplate;

    [SerializeField]
    private float speedofScrollingOnActivating = 2f;

    private bool isScrollingAnimation = false;

    private Coroutine AnimateScrollContentPositionCoroutineReference;

    public void ToggleParameter(bool isOn)
    {
        if (isOn)
        {
            //print("0. ToggleParameter — isOn == " + isOn);

            toggleActiveTemp = toggleGroup.ActiveToggles().FirstOrDefault();

            if (toggleActiveTemp)
            {
                if (int.TryParse(toggleActiveTemp.name, out int index))
                {
                    //print("1. ToggleParameter — Parsed Index: " + index);

                    ActivateParameter(index, true);
                }
                else
                {
                    print("1. ToggleParameter — not numeric name: "
                        + toggleActiveTemp.name);
                }
            }
            else
            {
                print("1. ToggleParameter — Toggle is Null.");
            }
        }
    }

    public void ActivateParameter(
        int index,
        bool isClickOnToggle,
        bool isAnimatedScrollOnSetToggleIsOnWithoutNotify = true)
    {
        //print("1.1. ActivateParameter — Index: " + index);

        if (index == toggleActivatedIndex)
        {
            //print("OnToggleSecondClick");

            AnimateScrollContentPosition(index, toggleActivatedIndex);

            OnToggleSecondClick?.Invoke(index);
        }
        else
        {
            if (isClickOnToggle)
            {
                AnimateScrollContentPosition(index, toggleActivatedIndex);

                ActivateParameterBase(index);
            }
            else if (SetToggleIsOnWithoutNotify(index))
            {
                if (isAnimatedScrollOnSetToggleIsOnWithoutNotify)
                {
                    AnimateScrollContentPosition(index, toggleActivatedIndex);
                }

                ActivateParameterBase(index);
            }
        }
    }

    private void ActivateParameterBase(int index)
    {
        toggleActivatedIndex = index;

        if (OnParameterActivating != null)
        {
            sliderActiveValue.value = OnParameterActivating.Invoke(index);
        }
    }

    public void ActivateOnScrolling()
    {
        if (!isScrollingAnimation)
        {
            int indexParameterOnCenter = Mathf.RoundToInt(
                GetParameterIndexRawFromScrollContentAnchoredPositionX());

            if (toggleActivatedIndex >= 0
                && indexParameterOnCenter >= 0
                && indexParameterOnCenter < toggles.Length)
            {
                //// Snapping
                //if (!isScrollDragging
                //    && Mathf.Abs(scrollRect.velocity.x) < speedofScrollingOnActivating / Time.fixedDeltaTime
                //    && indexParameterOnCenter > 0
                //    && indexParameterOnCenter < toggles.Length - 1)
                //{
                //    if (scrollRect.velocity.x > 0.001f)
                //    {
                //        indexParameterOnCenter--;
                //    }
                //    else if (scrollRect.velocity.x < -0.001f)
                //    {
                //        indexParameterOnCenter++;
                //    }

                //    SetScrollContentPosition(
                //        indexParameterOnCenter, toggleActivatedIndex);
                //}

                if (indexParameterOnCenter != toggleActivatedIndex)
                {
                    //print("OnScrollRectValueChanged: ActivateParameter: " + indexParameterOnCenter);

                    ActivateParameter(indexParameterOnCenter, false, false);
                }
            }
        }
    }

    private bool SetToggleIsOnWithoutNotify(int index)
    {
        //print("2. Toggle — SetIsOnWithoutNotify");

        for (int i = 0; i < toggles.Length; i++)
        {
            if (int.TryParse(toggles[i].name, out int indexCurrent))
            {
                if (index == indexCurrent)
                {
                    toggles[i].SetIsOnWithoutNotify(true);

                    return true;

                    //print("3. Toggle — Parsed Index: " + index);
                }
            }
            else
            {
                print("3. Toggle — not numeric name: " + toggles[i].name);
            }
        }

        return false;
    }

    public void SetParameterValue(float value)
    {
        //print("4. Toggle — Value: " + value);

        OnSetParameterValue?.Invoke(value);

        textActiveValue.text = value.ToString();
    }

    public void SetSliderValueWithoutNotify(int index, float value)
    {
        if (index == toggleActivatedIndex)
        {
            sliderActiveValue.SetValueWithoutNotify(value);

            textActiveValue.text = value.ToString();
        }
    }

    public IEnumerator InitCoroutine(Sprite[] sprites)
    {
        SetIcons(sprites);

        yield return new WaitForEndOfFrame();

        toggles = GetComponentsInChildren<Toggle>();

        SetScrollContentWidth();
    }

    private void SetIcons(Sprite[] sprites)
    {
        SetIcon(parameterButtonTemplate, sprites[0]);

        if (parameterButtonTemplate)
        {
            for (int i = 1; i < sprites.Length; i++)
            {
                RectTransform parameterButtonCurrent =
                    Instantiate(parameterButtonTemplate);

                parameterButtonCurrent.name = i.ToString();
                parameterButtonCurrent.SetParent(transform);
                parameterButtonCurrent.localScale = Vector3.one;

                SetIcon(parameterButtonCurrent, sprites[i]);
            }
        }
        else
        {
            Debug.Log("Parameter Button Template is Missing!");
        }
    }

    private void SetIcon(RectTransform button, Sprite sprite)
    {
        button.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    private void SetScrollContentWidth()
    {
        if (scrollRect)
        {
            scrollRect.content.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                rectTransform.sizeDelta.x
                    + canvas.sizeDelta.x
                    - parameterButtonTemplate.sizeDelta.x);
        }
        else
        {
            Debug.Log("Scroll Content is Missing!");
        }
    }

    public float GetParameterIndexRawFromScrollContentAnchoredPositionX()
    {
        float a = scrollRect.content.anchoredPosition.x
            - (rectTransform.sizeDelta.x - parameterButtonTemplate.sizeDelta.x) / 2f;

        float b =
            parameterButtonTemplate.sizeDelta.x + horizontalLayoutGroup.spacing;

        return a / -b;
    }

    public float GetScrollContentAnchoredPositionXFromParameterIndex(int index)
    {
        float a = (rectTransform.sizeDelta.x
            - parameterButtonTemplate.sizeDelta.x) / 2f;

        float b = (parameterButtonTemplate.sizeDelta.x
            + horizontalLayoutGroup.spacing) * index;

        return a - b;
    }


    public void OnScrollRectValueChanged()
    {
        //print("OnScrollRectValueChanged");

        ActivateOnScrolling();
    }

    //private bool isScrollDragging = false;

    public void OnScrollBeginDrag()
    {
        //isScrollDragging = true;
    }

    public void OnScrollEndDrag()
    {
        //isScrollDragging = false; 

        // if (velocity.x == 0) {here will be part of snapping logic}
    }

    private void AnimateScrollContentPosition(int index, int indexPrevious)
    {
        if (AnimateScrollContentPositionCoroutineReference != null)
        {
            StopCoroutine(AnimateScrollContentPositionCoroutineReference);
        }

        AnimateScrollContentPositionCoroutineReference =
            StartCoroutine(AnimateScrollContentPositionCoroutine(
                index, indexPrevious));
    }

    private IEnumerator AnimateScrollContentPositionCoroutine(
        int indexTarget, int indexPrevious)
    {
        isScrollingAnimation = true;

        if (scrollRect)
        {
            scrollRect.StopMovement();

            float anchoredPositionXTarget =
                GetScrollContentAnchoredPositionXFromParameterIndex(indexTarget);


            float indexParameterOnCenterRaw = GetParameterIndexRawFromScrollContentAnchoredPositionX();

            int indexParameterOnCenter = Mathf.RoundToInt(indexParameterOnCenterRaw);


            float speedFactor = indexTarget - indexParameterOnCenter;

            if (indexPrevious < 0)
            {
                speedFactor = -toggles.Length;
            }
            else if (Mathf.Abs(speedFactor) < 0.001f) // i.e. == 0 (center is nearby target parameter)
            {
                speedFactor = Mathf.Sign(indexParameterOnCenter - indexParameterOnCenterRaw);
            }

            while ((scrollRect.content.anchoredPosition.x - anchoredPositionXTarget) * Mathf.Sign(speedFactor) > 0f)
            {
                //print(index);

                float anchoredPositionXCurrent =
                    scrollRect.content.anchoredPosition.x - speedofScrollingOnActivating * speedFactor;

                if ((anchoredPositionXCurrent - anchoredPositionXTarget) * Mathf.Sign(speedFactor) < 0f)
                {
                    anchoredPositionXCurrent = anchoredPositionXTarget;
                }

                scrollRect.content.anchoredPosition = new Vector2(
                    anchoredPositionXCurrent,
                    0f);

                yield return null;
            } 
        }
        else
        {
            Debug.Log("Scroll Content is Missing!");
        }

        AnimateScrollContentPositionCoroutineReference = null;

        isScrollingAnimation = false;
    }
}
