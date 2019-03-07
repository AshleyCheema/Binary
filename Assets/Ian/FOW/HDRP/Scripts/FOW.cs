/*
 * Author: Ian Hudson
 * Description: HDRP - Use a post process effect to try and simulate fog of war
 * Created: 08/02/2019
 * Edited By: Ian
 */

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Create a post processing effect
/// </summary>
[Serializable]
[PostProcess(typeof(FOWRenderer), PostProcessEvent.AfterStack, "Custom/FOW")]
public sealed class FOW : PostProcessEffectSettings
{
    //Set the distance paramter
    [Range(0, 30)]
    public FloatParameter MaxDistance = new FloatParameter { value = 10.0f };
}

/// <summary>
/// Set the post process renderer
/// </summary>
public sealed class FOWRenderer : PostProcessEffectRenderer<FOW>
{
    /// <summary>
    /// Render the image with a material/shader
    /// </summary>
    /// <param name="context"></param>
    public override void Render(PostProcessRenderContext context)
    {
        Shader shader = Shader.Find("Custom/FOW");
        Material mat = new Material(shader);
        //sheet.properties.SetFloat("_Blend", settings.MaxDistance);
        context.command.Blit(context.source, context.destination, mat);
    }
}
