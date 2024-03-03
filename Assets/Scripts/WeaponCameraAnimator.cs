using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCameraAnimator : MonoBehaviour
{
    private const string ENABLE_SCOPE_FLOAT = "EnableScopeFloat";
    private PlayerWeapon playerWeapon;
    private Animator animator;
    private bool isScopeEnabled;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerWeapon = GetComponentInParent<PlayerWeapon>();
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

    // Start is called before the first frame update
    void Start()
    {

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
