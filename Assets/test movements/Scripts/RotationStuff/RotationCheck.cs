using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCheck : MonoBehaviour
{
    public Vector3 eulerAngles;
    public Vector3 rot;
    public Vector3 localRot;
    public Vector3 rot2Deg;
    public Vector3 localRot2Deg;

    private void Update()
    {
        //Finally!!!
        eulerAngles = transform.rotation.eulerAngles;

        //Crap
        rot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        localRot = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
        rot2Deg = new Vector3(transform.rotation.x * Mathf.Rad2Deg, transform.rotation.y * Mathf.Rad2Deg, transform.rotation.z * Mathf.Rad2Deg);
        localRot2Deg = new Vector3(transform.localRotation.x * Mathf.Rad2Deg, transform.localRotation.y * Mathf.Rad2Deg, transform.localRotation.z * Mathf.Rad2Deg);
    }
}
