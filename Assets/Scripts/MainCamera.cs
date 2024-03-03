using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private PlayerWeapon playerWeapon;
    private Camera cam;
    private bool isScopeEnabled;
    private void Awake()
    {
        playerWeapon = GetComponentInParent<PlayerWeapon>();
        cam = GetComponent<Camera>();
        playerWeapon.OnWeaponChanged += PlayerWeapon_OnWeaponChanged;
    }
    private void PlayerWeapon_OnWeaponChanged(object sender, PlayerWeapon.OnWeaponChangedEventArgs e)
    {
        isScopeEnabled = false;
        if (e.previousWeapon is Gun)
        {
            e.previousWeapon.GetComponent<Gun>().OnScopeEnabled -= PlayerGun_OnScopeEnabled;
            e.previousWeapon.GetComponent<Gun>().OnScopeDisabled -= PlayerGun_OnScopeDisabled;
        }
        if (playerWeapon.GetWeapon() is Gun)
        {
            playerWeapon.GetWeapon().GetComponent<Gun>().OnScopeEnabled += PlayerGun_OnScopeEnabled;
            playerWeapon.GetWeapon().GetComponent<Gun>().OnScopeDisabled += PlayerGun_OnScopeDisabled;
        }
    }

    private void PlayerGun_OnScopeDisabled(object sender, System.EventArgs e)
    {
        isScopeEnabled = false;
    }

    private void PlayerGun_OnScopeEnabled(object sender, System.EventArgs e)
    {
        isScopeEnabled = true;
    }
    void Start()
    {
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
