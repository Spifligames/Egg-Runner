using System.Collections;
using UnityEngine;

public class EffectPad : MonoBehaviour
{
    public enum PadEffect { JumpBoost, SpeedBoost }
    
    [Header("Effect Settings")]
    public PadEffect padEffect;
    public float effectDuration = 5f;
    public float speedMultiplier = 2f;
    public float jumpMultiplier = 2f;
    [Header("Object References")]
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    [SerializeField] private Material jumpPadMaterial;
    [SerializeField] private Material speedPadMaterial;

    private float initialPlayerWalkSpeed;
    private float initialPlayerRunSpeed;
    private float initialPlayerJumpSpeed;
    private Renderer objectRenderer;

    private void Start()
    {
        initialPlayerWalkSpeed = player.m_WalkSpeed;
        initialPlayerRunSpeed = player.m_RunSpeed;
        initialPlayerJumpSpeed = player.m_JumpSpeed;
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
            StartCoroutine(StartPadEffect());
        }
    }

    IEnumerator StartPadEffect()
    {
        float timeElapsed = 0;
        switch (padEffect)
        {
            case PadEffect.SpeedBoost:
                while (timeElapsed < effectDuration)
                {
                    player.m_WalkSpeed = initialPlayerWalkSpeed * speedMultiplier;
                    player.m_RunSpeed = initialPlayerRunSpeed * speedMultiplier;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                player.m_WalkSpeed = initialPlayerWalkSpeed;
                player.m_RunSpeed = initialPlayerRunSpeed;
                break;
            case PadEffect.JumpBoost:
                while (timeElapsed < effectDuration)
                {
                    player.m_JumpSpeed = initialPlayerJumpSpeed * jumpMultiplier;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                player.m_JumpSpeed = initialPlayerJumpSpeed;
                break;
        }
    }
}
