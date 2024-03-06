using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private PlayerWeapon playerWeapon;
    private Camera cam;
    private bool isScopeEnabled;
    private void Start()
    {
        playerWeapon = GetComponentInParent<PlayerWeapon>();
        cam = GetComponent<Camera>();
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

    void Update()
    {
        if (isScopeEnabled && cam.fieldOfView > 30)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,30,0.1f);
        }
        else if (!isScopeEnabled && cam.fieldOfView < 60)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, 0.1f);
        }
    }
}
