using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Transform cam;
    public float force;
    void Awake()
    {
        cam = GameObject.Find("Fps Cam").transform;
    }
    private void Start()
    {
        rb.AddForce(cam.forward * force);
    }
}
