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

[HelpURL("https://makaka.org/unity-assets")]
[Serializable]
public class ARMaskBaseParameterControl
{
    public string name;

    public string iconName;

    public float initial;

    private readonly float initialBase;

    public float min;

    public float max;

    public float Current { get; set; }

    public Action<float> OnParameterValueChanged;

    public Action<float, Material, int> OnMaterialChanged;

    public string shaderParameterName;

    public readonly static string SHADER_PARAMETER_NAME_ROTATION =
        "_RotationDegrees";

    public ARMaskBaseParameterControl(
        string name,
        string iconName,
        float initial,
        float min,
        float max,
        Action<float, Material, int> OnMaterialChanged,
        string shaderParameterName = null)
    {
        this.name = name;
        this.iconName = iconName;

        this.initial = this.initialBase = Current = initial;
        this.min = min;
        this.max = max;

        this.OnMaterialChanged = OnMaterialChanged;

        this.shaderParameterName = shaderParameterName;
    }

    public void ResetInitial()
    {
        initial = initialBase;
    }

    public float GetCurrentNormalized()
    {
        return NormalizeOnRangeWithClamp01(Current, min, max);
    }

    public float GetDenormalizedFrom(float value)
    {
        return DenormalizeOnRange(value, min, max);
    }

    private float NormalizeOnRangeWithClamp01(float value, float min, float max)
    {
        return Mathf.Clamp01((value - min) / (max - min));
    }

    private float DenormalizeOnRange(float value, float min, float max)
    {
        return min + value * (max - min);
    }
}
