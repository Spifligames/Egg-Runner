using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingHandler : MonoBehaviour
{ 
    [Header("General Settings")]
    [SerializeField] private bool usePostProcessingEffects = true;
    [SerializeField] private Volume postProcessingVolume;
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
        if (!postProcessingVolume.profile.TryGet(out _vignette)) throw new NullReferenceException(nameof(_vignette));
        postProcessingVolume.profile.TryGet(out _lensDistortion);
        initialCameraFov = playerCamera.fieldOfView;

        SetInitialSettings(blankConfig);
    }

    private void SetInitialSettings(PostProcessingEffectConfig config)
    {
        // Make sure the effects can be overridden during playtime
        _vignette.intensity.overrideState = true;
        _vignette.color.overrideState = true;
        _lensDistortion.intensity.overrideState = true;
        
        _vignette.intensity.Override(config.vignetteIntensity);
        _vignette.color.value = config.vignetteColor;
        _lensDistortion.intensity.value = config.lensDistortionIntensity;
        playerCamera.fieldOfView += config.cameraFovIncrease;
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

        Color currentVignetteColour = _vignette.color.value;
        float currentVignetteIntensity = _vignette.intensity.value;
        float currentLensDistortionIntensity = _lensDistortion.intensity.value;
        float currentFov = playerCamera.fieldOfView;
        float newFov = initialCameraFov + config.cameraFovIncrease;

        while (timeElapsed < config.fadeTime)
        {
            _vignette.color.value = Color.Lerp(currentVignetteColour, config.vignetteColor, timeElapsed / config.fadeTime);
            _vignette.intensity.value = Mathf.Lerp(currentVignetteIntensity, config.vignetteIntensity, timeElapsed / config.fadeTime);
            _lensDistortion.intensity.value = Mathf.Lerp(currentLensDistortionIntensity, config.lensDistortionIntensity, timeElapsed / config.fadeTime);
            playerCamera.fieldOfView = Mathf.Lerp(currentFov, newFov, timeElapsed / config.fadeTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        activeEffect = null;
    }
}
