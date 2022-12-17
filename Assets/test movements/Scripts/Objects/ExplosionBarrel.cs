using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : MonoBehaviour
{
    public float explosionForce, explosionRange, upwardsModifier;
    private Collider[] objectsToPush;
    public LayerMask whatIsPlayer;

    void Update()
    {
        objectsToPush = Physics.OverlapSphere(transform.position, explosionRange, whatIsPlayer);
    }
    public void PushExplosion()
    {
        for (int i = 0; i < objectsToPush.Length; i++)
        {
            objectsToPush[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange, upwardsModifier);
        }

        Destroy(gameObject);
    }
}
