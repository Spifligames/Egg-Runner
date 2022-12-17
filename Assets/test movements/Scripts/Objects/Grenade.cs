using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionForce, explosionRange, upwardsModifier, speed;
    private Collider[] objectsToPush;
    public LayerMask whatIsPlayer;

    void Update()
    {
        //fly forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        objectsToPush = Physics.OverlapSphere(transform.position, explosionRange, whatIsPlayer);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Wall")){
            PushExplosion();
        }
    }

    private void PushExplosion()
    {
        for (int i = 0; i < objectsToPush.Length; i++)
        {
            objectsToPush[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange, upwardsModifier);
        }

        Destroy(gameObject);
    }
}
