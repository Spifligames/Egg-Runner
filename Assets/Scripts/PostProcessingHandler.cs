using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingHandler : MonoBehaviour
{ 
    [Header("General Settings")]
    [SerializeField] private bool usePostProcessingEffects = true;
    [SerializeField] private PostProcessVolume postProcessingVolume;
    [SerializeField] private Camera playerCamera;
    private Coroutine activeEffect;

    [Header("Effects Config")] 
    private List<PostProcessingEffectConfig> activePadEffects = new List<PostProcessingEffectConfig>();
    [SerializeField] private PostProcessingEffectConfig blankConfig;
    private Vignette _vignette;
    private LensDistortion _lensDistortion;
    private float initialCameraFov;
    
    #region =====  SINGLETON INITIALISATION =====
    // Create a singleton out of the CameraFX script as it only needs to have one instance
    private static PostProcessingHandler _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;
    }
    public static PostProcessingHandler Instance => _instance;
    #endregion

    private void OnEnable()
    {
        postProcessingVolume.profile.TryGetSettings(out _vignette);
        postProcessingVolume.profile.TryGetSettings(out _lensDistortion);
        initialCameraFov = playerCamera.fieldOfView;
    }
    
    public void StartPadEffect(PostProcessingEffectConfig config)
    {
        if (!usePostProcessingEffects) return;
        
        if (activeEffect != null) StopCoroutine(activeEffect);
        activePadEffects.Add(config);
        activeEffect = StartCoroutine(FadePadFX(config));
    }

    public void StopPadEffect(PostProcessingEffectConfig config)
    {
        if (activeEffect != null) StopCoroutine(activeEffect);

        activePadEffects.Remove(config);
        activeEffect = StartCoroutine(activePadEffects.Count > 0 
            ? FadePadFX(activePadEffects[0]) 
            : FadePadFX(blankConfig));
    }

    private IEnumerator FadePadFX(PostProcessingEffectConfig config)
    {
        float timeElapsed = 0;

        Color currentVignetteColour = _vignette.color;
        float currentVignetteIntensity = _vignette.intensity;
        float currentLensDistortionIntensity = _lensDistortion.intensity;
        float currentFov = playerCamera.fieldOfView;
        float newFov = initialCameraFov + config.cameraFovIncrease;

        while (timeElapsed < config.fadeTime)
        {
            _vignette.color.Override(Color.Lerp(currentVignetteColour, config.vignetteColor, timeElapsed / config.fadeTime));
            _vignette.intensity.Override(Mathf.Lerp(currentVignetteIntensity, config.vignetteIntensity, timeElapsed / config.fadeTime));
            _lensDistortion.intensity.Override(Mathf.Lerp(currentLensDistortionIntensity, config.lensDistortionIntensity, timeElapsed / config.fadeTime));
            playerCamera.fieldOfView = Mathf.Lerp(currentFov, newFov, timeElapsed / config.fadeTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        activeEffect = null;
    }
}
