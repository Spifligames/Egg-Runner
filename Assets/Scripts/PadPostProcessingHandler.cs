using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PadPostProcessingHandler : MonoBehaviour
{
    [Header("General Settings")]
    public bool usePostProcessingEffects = true;
    [SerializeField] private float postProcessTransitionTime = 0.5f;
    [SerializeField] private float vignetteIntensity = 0.5f;
    [SerializeField] private float lensDistortionIntensity = 30f;
    [SerializeField] private float cameraFovIncrease = 10f;
    [SerializeField] private Color jumpVignetteColour = new Color(0, 0.25f, 1, 1);
    [SerializeField] private Color speedVignetteColour = Color.green;
    
    [Header("Object References")]
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    
    // General private variables
    private Camera playerCamera;
    
    // Post-processing specific private variables
    private PostProcessVolume ppVolume;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private float initialPlayerCameraFov;
    private EffectPad.PadEffect lastPadEffect;
    
    [HideInInspector] public bool effectActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (usePostProcessingEffects)
        {
            try
            {
                ppVolume = GameObject.FindGameObjectWithTag("PostProcessingVolume").GetComponent<PostProcessVolume>();
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning(
                    "ERROR: A post-processing volume couldn't be detected. The post-processing volume must exist in the scene and have the PostProcessingVolume tag.\nDetails: " +
                    e);
            }

            vignette = ppVolume.profile.GetSetting<Vignette>();
            vignette.intensity.value = 0;
            lensDistortion = ppVolume.profile.GetSetting<LensDistortion>();
            lensDistortion.intensity.value = 0;

            playerCamera = GameObject.Find("Camera").GetComponent<Camera>();
            initialPlayerCameraFov = playerCamera.fieldOfView;
        }
    }

    public IEnumerator PPEffectTransition(EffectPad.PadEffect effect, bool enable)
    {
        effectActive = true;
        float timeElapsed = 0;
        switch (effect)
        {
            case EffectPad.PadEffect.SpeedBoost:
                lastPadEffect = EffectPad.PadEffect.SpeedBoost;
                vignette.color.value = speedVignetteColour;
                
                while (timeElapsed < postProcessTransitionTime)
                {
                    switch (enable)
                    {
                        case true:
                            vignette.intensity.value = Mathf.Lerp(0, vignetteIntensity,
                                timeElapsed / postProcessTransitionTime);
                            lensDistortion.intensity.value = Mathf.Lerp(0, -lensDistortionIntensity,
                                timeElapsed / postProcessTransitionTime);
                            playerCamera.fieldOfView = Mathf.Lerp(initialPlayerCameraFov,
                                (initialPlayerCameraFov + cameraFovIncrease), timeElapsed / postProcessTransitionTime);
                            break;
                        case false:
                            vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0,
                                timeElapsed / postProcessTransitionTime);
                            lensDistortion.intensity.value = Mathf.Lerp(-lensDistortionIntensity, 0,
                                timeElapsed / postProcessTransitionTime);
                            playerCamera.fieldOfView = Mathf.Lerp((initialPlayerCameraFov + cameraFovIncrease), initialPlayerCameraFov, timeElapsed / postProcessTransitionTime);
                            break;
                    }
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                
                switch (enable)
                {
                    case true:
                        vignette.intensity.value = vignetteIntensity;
                        lensDistortion.intensity.value = -lensDistortionIntensity;
                        playerCamera.fieldOfView = (initialPlayerCameraFov + cameraFovIncrease);
                        break;
                    case false:
                        vignette.intensity.value = 0;
                        lensDistortion.intensity.value = 0;
                        playerCamera.fieldOfView = initialPlayerCameraFov;
                        effectActive = false;
                        break;
                }
                break;
            
            case EffectPad.PadEffect.JumpBoost:
                lastPadEffect = EffectPad.PadEffect.JumpBoost;
                Debug.Log(vignette.color.value);
                vignette.color.value = jumpVignetteColour;
                
                while (timeElapsed < postProcessTransitionTime)
                {
                    switch (enable)
                    {
                        case true:
                            Debug.Log("Transitioned into jump pad FX");
                            vignette.intensity.value = Mathf.Lerp(0, vignetteIntensity,
                                timeElapsed / postProcessTransitionTime);
                            break;
                        case false:
                            Debug.Log("Transitioned out of jump pad FX");
                            vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0,
                                timeElapsed / postProcessTransitionTime);
                            break;
                    }
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                
                switch (enable)
                {
                    case true:
                        vignette.intensity.value = vignetteIntensity;
                        break;
                    case false:
                        vignette.intensity.value = 0;
                        effectActive = false;
                        break;
                }
                break;
        }
    }
}
