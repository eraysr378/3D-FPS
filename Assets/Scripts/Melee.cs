using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{

    private bool isShooting;
    private float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shootTimer < GetTimeBetweenShots())
        {
            shootTimer += Time.deltaTime;
        }
    }
    public override void Shoot()
    {
        if (shootTimer < GetTimeBetweenShots())
        {
            return;
        }
        shootTimer = 0;
        isShooting = true;
        InvokeOnShootingStarted();

    }
    public override void StopShooting()
    {
        // this if added to fix animation when a single shot fired
        if (shootTimer >= GetTimeBetweenShots() && isShooting)
        {
            InvokeOnShootingEnd();
            isShooting = false;
        }

    }
}

