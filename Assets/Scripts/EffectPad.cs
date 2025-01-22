using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectPad : MonoBehaviour
{
    public enum PadEffect { JumpBoost, SpeedBoost }
    
    [Header("General Effect Settings")]
    public PadEffect padEffect;
    
    [Header("Speed Effect Settings")] 
    public float speedEffectDuration = 5f;
    public float speedMultiplier = 2f;
    
    [Header("Jump Effect Settings")]
    public float jumpMultiplier = 2f;
    
    [Header("Post Processing Settings")]
    public bool usePostProcessingEffects = true;
    [SerializeField] private float postProcessTransitionTime = 0.5f;
    [SerializeField] private float vignetteIntensity = 0.5f;
    [SerializeField] private float lensDistortionIntensity = 30f;
    [SerializeField] private float cameraFovIncrease = 10f;
    
    [Header("Object References")]
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    [SerializeField] private Material jumpPadMaterial;
    [SerializeField] private Material speedPadMaterial;

    // General private variables
    private bool effectActive = false;
    private bool isJumpEffectActive = false;
    private bool colliderTouched = false;
    private Camera playerCamera;
    private float initialPlayerCameraFov;
    private float initialPlayerWalkSpeed;
    private float initialPlayerRunSpeed;
    private float initialPlayerJumpSpeed;
    private float mainTimeElapsed = 0f;
    private Renderer objectRenderer;
    
    // Post-processing specific private variables
    private Color jumpVignetteColour = Color.green;
    private Color speedVignetteColour = new Color(0, 0.25f, 1, 1);
    private PostProcessVolume ppVolume;
    private Vignette vignette;
    private LensDistortion lensDistortion;

    private void Start()
    {
        initialPlayerWalkSpeed = player.m_WalkSpeed;
        initialPlayerRunSpeed = player.m_RunSpeed;
        initialPlayerJumpSpeed = player.m_JumpSpeed;
        playerCamera = GameObject.Find("Camera").GetComponent<Camera>();
        initialPlayerCameraFov = playerCamera.fieldOfView;

        if (usePostProcessingEffects)
        {
            try
            {
                ppVolume = GameObject.FindGameObjectWithTag("PostProcessingVolume").GetComponent<PostProcessVolume>();
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning("ERROR: A post-processing volume couldn't be detected. The post-processing volume must exist in the scene and have the PostProcessingVolume tag.\nDetails: " + e);
            }
            
            vignette = ppVolume.profile.GetSetting<Vignette>();
            vignette.intensity.value = 0;
            lensDistortion = ppVolume.profile.GetSetting<LensDistortion>();
            lensDistortion.intensity.value = 0;
        }
    }

    // This allows the colour of the effect pad to be changed when changing the type of pad, even when not in playmode.
    // Created with the help of ChatGPT: https://chatgpt.com/share/679075d3-64c8-8002-9715-33abecdd6fe6
    private void OnValidate()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            switch (padEffect)
            {
                case PadEffect.JumpBoost:
                    objectRenderer.sharedMaterial = jumpPadMaterial;
                    break;
                case PadEffect.SpeedBoost:
                    objectRenderer.sharedMaterial = speedPadMaterial;
                    break;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(StartPadEffect());
            StartCoroutine(StartPadEffect());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isJumpEffectActive = false;
    }

    IEnumerator StartPadEffect()
    {
        switch (padEffect)
        {
            case PadEffect.SpeedBoost:
                if (usePostProcessingEffects && !effectActive) StartCoroutine(PPEffectTransition(PadEffect.SpeedBoost, true));

                mainTimeElapsed = 0;
                while (mainTimeElapsed < speedEffectDuration) // Timer for the speed boost
                {
                    player.m_WalkSpeed = initialPlayerWalkSpeed * speedMultiplier;
                    player.m_RunSpeed = initialPlayerRunSpeed * speedMultiplier;
                    mainTimeElapsed += Time.deltaTime;
                    Debug.Log(mainTimeElapsed);
                    yield return null;
                }
                
                effectActive = false;
                if (usePostProcessingEffects) StartCoroutine(PPEffectTransition(PadEffect.SpeedBoost, false));
                player.m_WalkSpeed = initialPlayerWalkSpeed;
                player.m_RunSpeed = initialPlayerRunSpeed;
                break;
            case PadEffect.JumpBoost:
                player.m_JumpSpeed = initialPlayerRunSpeed * jumpMultiplier;
                isJumpEffectActive = true;

                while (isJumpEffectActive)
                {
                    Debug.Log("Jump effect active");
                    yield return null;
                }
                player.m_JumpSpeed = initialPlayerJumpSpeed;
                break;
        }
    }

    private IEnumerator PPEffectTransition(PadEffect effect, bool enable)
    {
        effectActive = true;
        float timeElapsed = 0;
        switch (effect)
        {
            case PadEffect.SpeedBoost:
                    
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
                                (initialPlayerCameraFov + 10f), timeElapsed / postProcessTransitionTime);
                            break;
                        case false:
                            vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0,
                                timeElapsed / postProcessTransitionTime);
                            lensDistortion.intensity.value = Mathf.Lerp(-lensDistortionIntensity, 0,
                                timeElapsed / postProcessTransitionTime);
                            playerCamera.fieldOfView = Mathf.Lerp((initialPlayerCameraFov + 10f), initialPlayerCameraFov, timeElapsed / postProcessTransitionTime);
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
                        playerCamera.fieldOfView = (initialPlayerCameraFov + 10f);
                        break;
                    case false:
                        vignette.intensity.value = 0;
                        lensDistortion.intensity.value = 0;
                        playerCamera.fieldOfView = initialPlayerCameraFov;
                        break;
                }
                break;
        }
    }
}