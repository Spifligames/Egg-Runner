using System.Collections;
using UnityEngine;

public class SpeedPad : EffectPad
{
    [Header("SpeedPad/Effect Settings")]
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float speedEffectTime = 3f;
    
    // General private variables
    private float initialPlayerWalkSpeed;
    private float initialPlayerRunSpeed;
    private float timeElapsed = 0;
    private bool exitedTrigger;
    private Coroutine effectCoroutine;

    protected override void EnterEffectPad()
    {
        Debug.Log("Starting effect...");
        // Start the post-processing coroutine to transition into the effects if post-processing is enabled.
        if (initialPlayerWalkSpeed == 0)
            initialPlayerWalkSpeed = player.m_WalkSpeed;
        if (initialPlayerRunSpeed == 0)
            initialPlayerRunSpeed = player.m_RunSpeed;
        
        PostProcessingHandler.Instance.StartPadEffect(effectConfig);
        exitedTrigger = false;
        if (effectCoroutine == null)
            effectCoroutine = StartCoroutine(SpeedEffectTimer());
    }

    protected override void ExitEffectPad()
    {
        Debug.Log("Exiting effect...");
        timeElapsed = 0;
        exitedTrigger = true;
    }

    private IEnumerator SpeedEffectTimer()
    {
        timeElapsed = 0;
        player.m_WalkSpeed = initialPlayerWalkSpeed * speedMultiplier;
        player.m_RunSpeed = initialPlayerRunSpeed * speedMultiplier;

        while (timeElapsed < speedEffectTime)
        {
            if (exitedTrigger) timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        player.m_WalkSpeed = initialPlayerWalkSpeed;
        player.m_RunSpeed = initialPlayerRunSpeed;
        PostProcessingHandler.Instance.StopPadEffect(effectConfig);
        effectCoroutine = null;
    }
}
