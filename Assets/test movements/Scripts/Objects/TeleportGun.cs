using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportGun : MonoBehaviour
{
    public GameObject player;
    public GameObject hologram;
    public Transform objectToTeleport, cam;
    public float range, cooldown;
    bool readyToTeleport;
    RaycastHit rayHit;

    private void Awake()
    {
        readyToTeleport = true;
    }
    void Update()
    {
        if (Physics.Raycast(cam.position,cam.forward,out rayHit, range) && readyToTeleport)
        {
            hologram.SetActive(true);
            hologram.transform.position = rayHit.point;

            if (Input.GetKeyDown(KeyCode.Mouse0)){
                objectToTeleport.transform.position = rayHit.point;
                readyToTeleport = false;
                Invoke("ResetTeleport", cooldown);

                //player.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity / 4;
            }

            Debug.DrawRay(cam.position, cam.forward, Color.red);
        }
        else
            hologram.SetActive(false);
    }
    private void ResetTeleport()
    {
        readyToTeleport = true;
    }
}
