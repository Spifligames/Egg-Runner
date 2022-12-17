using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject grappleGun;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
            DeactivateAll();

        if (Input.GetKeyDown(KeyCode.Alpha1)) weapons[0].SetActive(true);
        if (Input.GetKeyDown(KeyCode.Alpha2)) grappleGun.GetComponent<GrapplingGun>().isActive = true;
        if (Input.GetKeyDown(KeyCode.Alpha3)) weapons[2].SetActive(true);
        if (Input.GetKeyDown(KeyCode.Alpha4)) weapons[3].SetActive(true);
        if (Input.GetKeyDown(KeyCode.Alpha5)) weapons[4].SetActive(true);
    }

    private void DeactivateAll()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            //exclude grappleGun
            if (i != 1)
                weapons[i].SetActive(false);
            else
                grappleGun.GetComponent<GrapplingGun>().isActive = false;
        }
    }
}
