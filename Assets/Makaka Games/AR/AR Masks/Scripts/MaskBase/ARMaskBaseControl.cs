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

using System;

#pragma warning disable 649

[HelpURL("https://makaka.org/unity-assets")]
public class ARMaskBaseControl : MonoBehaviour
{
    protected string nameARMask;

    private Sprite[] icons;


    protected ARMaskBaseParameterControl[] arMaskParameterControls;

    private int parameterActivatedIndex;

    public Action OnParametersReset;


    private Color colorForAlphaChangingTemp;

    private string colorForAlphaChangingParameterName = "_Color";


    private Texture textureTemp = null;

    private bool isClamp = false;


    public Action<string> OnStatusSet;

    private const string StatusTextureModeClamp = "Texture Mode: Clamp";
    private const string StatusTextureModeRepeat = "Texture Mode: Repeat";


    public Action<int, float> OnSetParameterValueWithoutClickingOnSlider;

    public Action OnScreenTap;

    public virtual void Init()
    {   
        InitIcons();
    }

    private void SetStatusText(string status)
    {
        OnStatusSet?.Invoke(status);
    }

    public int GetParametersCount()
    {
        if (arMaskParameterControls == null)
        {
            return 0;
        }
        else
        {
            return arMaskParameterControls.Length;
        }
    }

    private bool IsParameterIndexCorrect(int index)
    {
        if (index >= 0 && index < arMaskParameterControls.Length)
        {
            return true;
        }
        else
        {
            print("Wrong Parameter Index! " + index);
        
            return false;
        } 
    }

    private bool ActivateParameter(int index)
    {
        if (IsParameterIndexCorrect(index))
        {
            parameterActivatedIndex = index;

            SetStatusText(GetParameterName(index));

            return true;
        }
        else
        {
            return false;
        }
    }

    public float ActivateParameterAndGetNormilized(int index)
    {
        if (ActivateParameter(index))
        {
            return GetParameter(index).GetCurrentNormalized();
        }
        else
        {
            return -1f;
        }
    }

    private ARMaskBaseParameterControl GetParameterActivated()
    {
        return GetParameter(parameterActivatedIndex);
    }

    public ARMaskBaseParameterControl GetParameter(int index)
    {
        return IsParameterIndexCorrect(index)
            ? arMaskParameterControls[index]
            : null;
    }

    public string GetParameterName(int index)
    {
        return GetParameter(index)?.name;
    }

    private int GetParameterIndexByShaderParameterName(string name)
    {
        for (int i = 0; i < arMaskParameterControls.Length; i++)
        {
            if (arMaskParameterControls[i].shaderParameterName == name)
            {
                return i;
            }
        }

        return -1;
    }

    public string GetName()
    {
        return nameARMask;
    }

    private void InitIcons()
    {
        if (arMaskParameterControls != null)
        {
            icons = new Sprite[arMaskParameterControls.Length];

            for (int i = 0; i < arMaskParameterControls.Length; i++)
            {
                icons[i] = Resources.Load<Sprite>(
                    "AR Masks/" + GetName() + "/Parameters/"
                    + arMaskParameterControls[i].iconName);
            }
        }
    }

    public Sprite[] GetIcons()
    {
        return icons;
    }

    public bool SetActivatedParameterFromNormalized(float value)
    {
        return SetParameter(
            parameterActivatedIndex,
            GetParameterActivated().GetDenormalizedFrom(value));
    }

    private bool SetParameter(int index, float value, bool isNewInitial = false)
    {
        if (IsParameterIndexCorrect(index))
        {
            if (isNewInitial)
            {
                arMaskParameterControls[index].initial = value;
            }

            arMaskParameterControls[index].Current = value;
            arMaskParameterControls[index].OnParameterValueChanged?.Invoke(value);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SetParameterWithoutClickingOnSliderByShaderParameterName(
        string name, float value, bool isNewInitial = false)
    {
        int index = GetParameterIndexByShaderParameterName(name);

        if (SetParameter(index, value, isNewInitial))
        {
            OnSetParameterValueWithoutClickingOnSlider?.Invoke(
                index, GetParameter(index).GetCurrentNormalized());

            return true;
        }
        else
        {
            return false;
        }
    }


    private void ResetParameterInitial(int index)
    {
        if (IsParameterIndexCorrect(index))
        {
            arMaskParameterControls[index].ResetInitial();
        }
    }

    private void ResetParameter(int index, bool isResetInitial = false)
    {
        if (isResetInitial)
        {
            ResetParameterInitial(index);
        }

        SetParameter(index, arMaskParameterControls[index].initial);
    }

    public void ResetParameterWithoutClickingOnSlider(int index)
    {
        ResetParameter(index);

        OnSetParameterValueWithoutClickingOnSlider?.Invoke(
                index, GetParameter(index).GetCurrentNormalized());

        SetStatusText(GetParameterName(index) + ": Reset");
    }

    public void ResetParametersCompletely()
    {
        for (int i = 0; i < arMaskParameterControls.Length; i++)
        {
            ResetParameter(i, true);
        }

        OnParametersReset?.Invoke();
    }

    public void ResetTexture(Texture texture)
    {
        textureTemp = texture;

        if (isClamp)
        {
            SetTextureWrapModeToggle();
        }
    }


    protected void SetTilingX(float value, Material material, int indexParameter)
    {
        material.mainTextureScale = new Vector2(value, material.mainTextureScale.y);
    }

    protected void SetTilingY(float value, Material material, int indexParameter)
    {
        material.mainTextureScale = new Vector2(material.mainTextureScale.x, value);
    }

    protected void SetOffsetX(float value, Material material, int indexParameter)
    {
        material.mainTextureOffset = new Vector2(value, material.mainTextureOffset.y);
    }

    protected void SetOffsetY(float value, Material material, int indexParameter)
    {
        material.mainTextureOffset = new Vector2(material.mainTextureOffset.x, value);
    }

    protected void SetColorAlpha(float value, Material material, int indexParameter)
    {
        colorForAlphaChangingTemp = material.color;

        colorForAlphaChangingTemp.a = value;

        material.SetColor(
            colorForAlphaChangingParameterName, colorForAlphaChangingTemp);
    }

    protected void SetShaderParameter(
        float value,
        Material material, int indexParameter)
    {
        material.SetFloat(GetParameter(indexParameter).shaderParameterName, value);
    }

    protected void SetTextureWrapModeToggle()
    {
        isClamp = !isClamp;

        if (isClamp)
        {
            textureTemp.wrapMode = TextureWrapMode.Clamp;

            SetStatusText(StatusTextureModeClamp);
        }
        else
        {
            textureTemp.wrapMode = TextureWrapMode.Repeat;

            SetStatusText(StatusTextureModeRepeat);
        }
    }

}
