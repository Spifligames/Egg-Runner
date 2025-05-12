using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OldPostProcessingHandler : MonoBehaviour
{
    [Header("General Settings")]
    public bool usePostProcessingEffects = true;
    [Range(0.1f, 0.5f)][SerializeField] private float postProcessTransitionTime = 0.5f;
    [Range(0.0f, 1f)][SerializeField] private float vignetteIntensity = 0.5f;
    [Range(-50f, 50f)][SerializeField] private float lensDistortionIntensity = 30f;
    [Range(-30f, 30f)][SerializeField] private float cameraFovIncrease = 10f;
    [SerializeField] private Color jumpVignetteColour = new Color(0, 0.25f, 1, 1);
    [SerializeField] private Color speedVignetteColour = Color.green;
    [HideInInspector] public bool isSpeedEffectActive = false;
    
    [Header("Object References")]
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    
    // General private variables
    private Camera playerCamera;
    
    // Post-processing specific private variables
    private PostProcessVolume ppVolume;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private float initialPlayerCameraFov;
    
    [HideInInspector] public bool effectActive = false;
    
    #region =====  SINGLETON INITIALISATION =====
    // Create a singleton out of the CameraFX script as it only needs to have one instance
    private static OldPostProcessingHandler _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;
    }
    public static OldPostProcessingHandler Instance => _instance;
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Trys to find the post-processing volume in the scene and assign it to a private variable. The try catch method is used in case Damian forgets to import the volume into a scene.
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

            // Initialising private variables
            vignette = ppVolume.profile.GetSetting<Vignette>();
            vignette.intensity.value = 0;
            lensDistortion = ppVolume.profile.GetSetting<LensDistortion>();
            lensDistortion.intensity.value = 0;

            playerCamera = GameObject.Find("Camera").GetComponent<Camera>();
            initialPlayerCameraFov = playerCamera.fieldOfView;
        }
    }

    /// <summary>
    /// Starts a transition for the effect pad post-processing effects. Calling enable true starts a transition in, whereas false starts a transition out.
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="enable"></param>
    /// <returns></returns>
    public IEnumerator PPEffectTransition(OldEffectPad.PadEffect effect, bool enable)
    {
        effectActive = true;
        float timeElapsed = 0;
        switch (effect)
        {
            // ##### SPEED BOOST FX #####
            case OldEffectPad.PadEffect.SpeedBoost:
                if (enable) isSpeedEffectActive = true;
                else isSpeedEffectActive = false;
                
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
            // ##### JUMP BOOST FX #####
            case OldEffectPad.PadEffect.JumpBoost:
                Debug.Log(vignette.color.value);
                
                while (timeElapsed < postProcessTransitionTime)
                {
                    switch (enable)
                    {
                        case true:
                            if (isSpeedEffectActive)
                            {
                                Debug.Log("Transitioned into jump pad FX with speed effect active");
                                vignette.color.value = Color.Lerp(speedVignetteColour, jumpVignetteColour,
                                    timeElapsed / postProcessTransitionTime);
                            }
                            else
                            {
                                vignette.color.value = jumpVignetteColour;
                                Debug.Log("Transitioned into jump pad FX");
                                vignette.intensity.value = Mathf.Lerp(0, vignetteIntensity,
                                    timeElapsed / postProcessTransitionTime);
                            }
                            break;
                        case false:
                            if (isSpeedEffectActive)
                            {
                                Debug.Log("Transitioned out of jump pad FX with speed effect active");
                                vignette.color.value = Color.Lerp(jumpVignetteColour, speedVignetteColour,
                                    timeElapsed / postProcessTransitionTime);
                            }
                            else
                            {
                                Debug.Log("Transitioned out of jump pad FX");
                                vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0,
                                    timeElapsed / postProcessTransitionTime);
                            }
                            break;
                    }
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                
                switch (enable)
                {
                    case true:
                        if (isSpeedEffectActive)
                        {
                            vignette.color.value = jumpVignetteColour;
                        }
                        else
                        {
                            vignette.intensity.value = vignetteIntensity;
                        }
                        break;
                    case false:
                        if (isSpeedEffectActive)
                        {
                            vignette.color.value = speedVignetteColour;
                        }
                        else
                        {
                            vignette.intensity.value = 0;
                            effectActive = false;
                        }
                        break;
                }
                break;
        }
    }
}
