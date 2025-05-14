using UnityEngine;

[CreateAssetMenu(menuName = "EffectPad/PostProcessingConfig", fileName = "NewEffectConfig")]
public class PostProcessingEffectConfig : ScriptableObject
{
    public Color vignetteColor = Color.white;
    [Range(0, 1)] public float vignetteIntensity = 0.5f;
    [Range(-1, 1)] public float lensDistortionIntensity = -30f;
    [Range(-30, 30)] public float cameraFovIncrease = 10f;
    public float fadeTime = 0.5f;
}
