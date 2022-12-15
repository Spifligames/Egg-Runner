using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        Player.transform.parent = transform;
    }
    private void OnTriggerExit(Collider other)
    {
        Player.transform.parent = null;
    }
}