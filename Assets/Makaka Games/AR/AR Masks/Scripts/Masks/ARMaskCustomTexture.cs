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

#pragma warning disable 649

[HelpURL("https://makaka.org/unity-assets")]
public class ARMaskCustomTexture : ARMaskBaseControl
{   
    public override void Init()
    {
        nameARMask = "Custom Texture";

        arMaskParameterControls = new ARMaskBaseParameterControl[]
        {
            new ARMaskBaseParameterControl(
                "Offset X", "Icon X", 0f, -2f, 2f, SetOffsetX),

            new ARMaskBaseParameterControl(
                "Offset Y", "Icon Y", 0f, -2f, 2f, SetOffsetY),

            new ARMaskBaseParameterControl(
                "Tiling X", "Icon X", 1f, -8f, 8f, SetTilingX),

            new ARMaskBaseParameterControl(
                "Tiling Y", "Icon Y", 1f, -8f, 8f, SetTilingY),

            new ARMaskBaseParameterControl(
                "Transparency", "Icon T", 1f, 0f, 1f, SetColorAlpha),

            new ARMaskBaseParameterControl(
                "Rotation", "Icon R", 0f, -Mathf.PI, Mathf.PI,
                SetShaderParameter,
                ARMaskBaseParameterControl.SHADER_PARAMETER_NAME_ROTATION)
        };

        OnScreenTap = () => SetTextureWrapModeToggle();

        base.Init();
    }
}
