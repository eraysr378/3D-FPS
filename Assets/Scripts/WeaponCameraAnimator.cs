using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCameraAnimator : MonoBehaviour
{
    private const string ENABLE_SCOPE_FLOAT = "EnableScopeFloat";
    private PlayerWeapon playerWeapon;
    private Animator animator;
    private bool isScopeEnabled;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerWeapon = GetComponentInParent<PlayerWeapon>();
        playerWeapon.OnWeaponChanged += PlayerWeapon_OnWeaponChanged;
        Gun.OnScopeEnabled += Gun_OnScopeEnabled;
        Gun.OnScopeDisabled += Gun_OnScopeDisabled;

    }

    private void Gun_OnScopeDisabled(object sender, System.EventArgs e)
    {
        isScopeEnabled = false;
    }

    private void Gun_OnScopeEnabled(object sender, System.EventArgs e)
    {
        isScopeEnabled = true;
    }

    private void PlayerWeapon_OnWeaponChanged(object sender, PlayerWeapon.OnWeaponChangedEventArgs e)
    {
        isScopeEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScopeEnabled)
        {
            animator.SetFloat(ENABLE_SCOPE_FLOAT, 1, 0.05f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(ENABLE_SCOPE_FLOAT, 0, 0.05f, Time.deltaTime);
        }
    }
}
