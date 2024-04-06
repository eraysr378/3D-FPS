using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    static public event EventHandler OnSecondaryAttackStarted;
    static public event EventHandler OnSecondaryAttackEnd;
    [SerializeField] private float secondaryAttackDuration;
    [SerializeField]  private bool isSecondaryAttacking;
    private float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > secondaryAttackDuration && isSecondaryAttacking)
        {
            isSecondaryAttacking = false;
            OnSecondaryAttackEnd?.Invoke(this,EventArgs.Empty);
        }
    }
    public override void ChangeWeapon()
    {
        isSecondaryAttacking = false;
        ForceStopShooting();
        OnSecondaryAttackEnd?.Invoke(this, EventArgs.Empty);
    }
    public override void Shoot()
    {
        if (shootTimer < GetTimeBetweenShots() || isSecondaryAttacking)
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
    public override void RightClickAction()
    {
        if (shootTimer < secondaryAttackDuration)
        {
            return;
        }
        shootTimer = 0;
        isSecondaryAttacking = true;
        OnSecondaryAttackStarted?.Invoke(this, EventArgs.Empty);
    }

}

