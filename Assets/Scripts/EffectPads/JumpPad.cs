using UnityEngine;

public class JumpPad : EffectPad
{
    [Header("JumpPad/Effect Settings")]
    public float jumpMultiplier = 2f;
    
    // General private variables
    private float initialPlayerJumpSpeed;

    protected override void EnterEffectPad()
    {
        Debug.Log("Starting effect...");
        // Start the post-processing coroutine to transition into the effects if post-processing is enabled.
        if (initialPlayerJumpSpeed == 0)
            initialPlayerJumpSpeed = player.m_JumpSpeed;
        
        PostProcessingHandler.Instance.StartPadEffect(effectConfig);
        player.m_JumpSpeed = initialPlayerJumpSpeed * jumpMultiplier;
    }

    protected override void ExitEffectPad()
    {
        Debug.Log("Exiting effect...");
        PostProcessingHandler.Instance.StopPadEffect(effectConfig);
        player.m_JumpSpeed = initialPlayerJumpSpeed;
    }
}
