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
    
    [Header("Object References")]
    [SerializeField] private Material jumpPadMaterial;
    [SerializeField] private Material speedPadMaterial;

    // General private variables
    private bool isJumpEffectActive = false;
    private float initialPlayerWalkSpeed;
    private float initialPlayerRunSpeed;
    private float initialPlayerJumpSpeed;
    private float mainTimeElapsed = 0f;
    private Renderer objectRenderer;
    private Coroutine lastRoutine = null;
    private PadPostProcessingHandler _ppHandler;

    private void Start()
    {
        try
        {
            _ppHandler = GameObject.FindGameObjectWithTag("PostProcessingHandler").GetComponent<PadPostProcessingHandler>();
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("ERROR: A post-processing handler couldn't be detected. Post-processing will not work at all without a post-processing handler present in the scene.\nDetails: " + e);
        }
        
        initialPlayerWalkSpeed = _ppHandler.player.m_WalkSpeed;
        initialPlayerRunSpeed = _ppHandler.player.m_RunSpeed;
        initialPlayerJumpSpeed = _ppHandler.player.m_JumpSpeed;
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
            if (lastRoutine != null) StopCoroutine(lastRoutine);
            lastRoutine = StartCoroutine(StartPadEffect());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (padEffect == PadEffect.JumpBoost) isJumpEffectActive = false;
    }

    IEnumerator StartPadEffect()
    {
        switch (padEffect)
        {
            case PadEffect.SpeedBoost:
                if (_ppHandler.usePostProcessingEffects && !_ppHandler.effectActive) StartCoroutine(_ppHandler.PPEffectTransition(PadEffect.SpeedBoost, true));

                mainTimeElapsed = 0;
                while (mainTimeElapsed < speedEffectDuration) // Timer for the speed boost
                {
                    _ppHandler.player.m_WalkSpeed = initialPlayerWalkSpeed * speedMultiplier;
                    _ppHandler.player.m_RunSpeed = initialPlayerRunSpeed * speedMultiplier;
                    mainTimeElapsed += Time.deltaTime;
                    Debug.Log(mainTimeElapsed);
                    yield return null;
                }
                
                _ppHandler.effectActive = false;
                if (_ppHandler.usePostProcessingEffects) StartCoroutine(_ppHandler.PPEffectTransition(PadEffect.SpeedBoost, false));
                _ppHandler.player.m_WalkSpeed = initialPlayerWalkSpeed;
                _ppHandler.player.m_RunSpeed = initialPlayerRunSpeed;
                break;
            case PadEffect.JumpBoost:
                if (_ppHandler.usePostProcessingEffects && !_ppHandler.effectActive) StartCoroutine(_ppHandler.PPEffectTransition(PadEffect.JumpBoost, true));
                
                _ppHandler.player.m_JumpSpeed = initialPlayerRunSpeed * jumpMultiplier;
                isJumpEffectActive = true;

                while (isJumpEffectActive)
                {
                    //Debug.Log("Jump effect active");
                    yield return null;
                }
                
                Debug.Log("Stepped off jump pad");
                if (_ppHandler.usePostProcessingEffects) StartCoroutine(_ppHandler.PPEffectTransition(PadEffect.JumpBoost, false));
                _ppHandler.player.m_JumpSpeed = initialPlayerJumpSpeed;
                break;
        }
    }
}