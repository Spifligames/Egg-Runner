using System;
using UnityEngine;

public class EffectPad : MonoBehaviour
{
    [Header("EffectPad/Pad Material")] 
    [SerializeField] protected Material padMaterial;
    
    [Header("EffectPad/Post-processing Config")] 
    [SerializeField] protected PostProcessingEffectConfig effectConfig;

    [Header("EffectPad/Object References")]
    [SerializeField] private EffectPadTrigger trigger;
    protected PostProcessingHandler ppHandler;
    protected UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    private Renderer objectRenderer;
    
    // Internal values
    protected bool playerOnPad = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Tries to get the Post Processing handler from the scene. If the handler is not present in the scene, it'll produce an error message.
        try
        {
            ppHandler = PostProcessingHandler.Instance;
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("ERROR: A post-processing handler couldn't be detected. Post-processing will not work at all without a post-processing handler present in the scene.\nDetails: " + e);
        }
    }

    protected void OnValidate()
    {
        if (objectRenderer == null) objectRenderer = trigger.GetComponent<Renderer>();
        objectRenderer.sharedMaterial = padMaterial;
    }

    public void EffectPadActivate(UnityStandardAssets.Characters.FirstPerson.FirstPersonController other)
    {
        if (player == null)
            player = other;

        playerOnPad = true;
        EnterEffectPad();
    }

    public void EffectPadDeactivate()
    {
        playerOnPad = false;
        ExitEffectPad();
    }

    protected virtual void EnterEffectPad()
    {
        
    }

    protected virtual void ExitEffectPad()
    {
        
    }
}