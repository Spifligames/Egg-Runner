using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform Cam;
    public GameObject Player;

    public bool IsUsingRayCasts;
    public bool IsUsingBullets;

    public Transform ShootingPoint;
    public GameObject Bullet;
    float timeBetweenShooting;
    public float startTimeBetweenShooting;
    public int damage = 25;
    public float range = 100f;
    public float spread = 0f;

    //public ParticleSystem ShootEffect;
    public void Shoot()
    {  
        if (IsUsingRayCasts)
        {
            RaycastHit hit;
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, range))
            {
                if (hit.collider.CompareTag("ExplosionBarrel"))
                    hit.collider.GetComponent<ExplosionBarrel>().PushExplosion();

                Debug.Log(hit.collider.gameObject.name);
            }
        }

        if (IsUsingBullets)
        {
            //Calculate Direction and Spread
            //Quaternion Direction = Cam.transform.rotation + Quaternion.Euler(spread, spread, 0);

            Instantiate(Bullet, ShootingPoint.position, Cam.transform.rotation);
        }
    }
    private void Update()
    {
        //Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0) && timeBetweenShooting <= 0)
        {
            Shoot();
            timeBetweenShooting = startTimeBetweenShooting;
        }
        else
        {
            timeBetweenShooting -= Time.deltaTime;
        }
    }
}
