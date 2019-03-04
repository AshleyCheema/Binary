/*
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(FOWRenderer), PostProcessEvent.AfterStack, "Custom/FOW")]
public sealed class FOW : PostProcessEffectSettings
{
    [Range(0, 30)]
    public FloatParameter MaxDistance = new FloatParameter { value = 10.0f };
}

public sealed class FOWRenderer : PostProcessEffectRenderer<FOW>
{
    public override void Render(PostProcessRenderContext context)
    {
        Shader shader = Shader.Find("Custom/FOW");
        Material mat = new Material(shader);
        //sheet.properties.SetFloat("_Blend", settings.MaxDistance);
        context.command.Blit(context.source, context.destination, mat);
    }
}*/
