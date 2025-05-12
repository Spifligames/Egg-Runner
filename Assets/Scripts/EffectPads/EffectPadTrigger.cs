using System;
using UnityEngine;

public class EffectPadTrigger : MonoBehaviour
{
    [SerializeField] private EffectPad parentEffectPad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
            parentEffectPad.EffectPadActivate(
                other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()
                );
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
            parentEffectPad.EffectPadDeactivate();
    }
}
