
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string RELOAD = "Reload";
    private const string ENABLE_SCOPE_FLOAT = "EnableScopeFloat";
    private const string ENABLE_SCOPE = "EnableScope";
    private const string IS_SHOOTING = "IsShooting";
    private const string SHOOT_TRIGGER = "ShootTrigger";
    private const string RELOAD_TRIGGER = "ReloadTrigger";

    private const string X = "x";
    private const string Y = "y";
    [SerializeField] private Animator animator;

    private PlayerMotor playerMotor;
    private PlayerWeapon playerWeapon;
    // Start is called before the first frame update
    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerWeapon.OnWeaponChanged += PlayerWeapon_OnWeaponChanged;
        playerWeapon.OnWeaponPulled += PlayerWeapon_OnWeaponPulled;


    }

    private void PlayerWeapon_OnWeaponPulled(object sender, System.EventArgs e)
    {
        ResetIdleAnimation();
    }

    private void PlayerWeapon_OnWeaponChanged(object sender, PlayerWeapon.OnWeaponChangedEventArgs e)
    {
        UnsubscribeFromWeapon(e.previousWeapon);
        SubscribeToWeapon(playerWeapon.GetWeapon());
        animator.SetTrigger("ChangeGun");
        animator.SetTrigger(playerWeapon.GetWeapon().gameObject.name);
        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
    }

    private void PlayerGun_OnShootingEnd(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_SHOOTING, false);
        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
    }

    private void PlayerGun_OnShootingStarted(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_SHOOTING, true);
        animator.SetTrigger(SHOOT_TRIGGER);
    }

    private void PlayerGun_OnScopeDisabled(object sender, System.EventArgs e)
    {
        animator.SetBool(ENABLE_SCOPE, false);
    }

    private void PlayerGun_OnScopeEnabled(object sender, System.EventArgs e)
    {
        animator.SetBool(ENABLE_SCOPE, true);
    }

    private void PlayerGun_OnReloadCancel(object sender, System.EventArgs e)
    {
        animator.SetBool(RELOAD, false);
        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
    }
    private void PlayerGun_OnReloadEnd(object sender, System.EventArgs e)
    {
        animator.SetBool(RELOAD, false);

        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();


    }
    private void PlayerGun_OnReloadStart(object sender, System.EventArgs e)
    {
        animator.SetBool(RELOAD, true);
        animator.SetTrigger(RELOAD_TRIGGER);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool(ENABLE_SCOPE) && animator.GetFloat(ENABLE_SCOPE_FLOAT) < 1)
        {
            animator.SetFloat(ENABLE_SCOPE_FLOAT, animator.GetFloat(ENABLE_SCOPE_FLOAT) + Time.deltaTime * 6);
        }
        else if (!animator.GetBool(ENABLE_SCOPE) && animator.GetFloat(ENABLE_SCOPE_FLOAT) > 0)
        {
            animator.SetFloat(ENABLE_SCOPE_FLOAT, animator.GetFloat(ENABLE_SCOPE_FLOAT) - Time.deltaTime * 6);
        }

        if (playerMotor.IsSprinting() && playerMotor.IsWalking())
        {
            float x = animator.GetFloat(X);
            float y = animator.GetFloat(Y);
            if (x > 0)
            {
                animator.SetFloat(X, x - Time.deltaTime * 4);

            }
            if (y < 1)
            {
                animator.SetFloat(Y, y + Time.deltaTime * 4);
            }
        }
        else
        {

            float x = animator.GetFloat(X);
            float y = animator.GetFloat(Y);
            if (x > 0)
            {
                animator.SetFloat(X, x - Time.deltaTime * 5);

            }
            if (y > 0)
            {
                animator.SetFloat(Y, y - Time.deltaTime * 5);
            }
        }

    }
    private void ResetIdleAnimation()
    {
        animator.SetFloat(X, 0);
        animator.SetFloat(Y, 0);
    }
    private void UnsubscribeFromWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            return;
        }
        weapon.OnShootingStarted -= PlayerGun_OnShootingStarted;
        weapon.OnShootingEnd -= PlayerGun_OnShootingEnd;
        Gun gun = weapon.GetComponent<Gun>();
        if (gun != null)
        {
            gun.OnReloadStart -= PlayerGun_OnReloadStart;
            gun.OnReloadEnd -= PlayerGun_OnReloadEnd;
            gun.OnReloadCancel -= PlayerGun_OnReloadCancel;
            gun.OnScopeEnabled -= PlayerGun_OnScopeEnabled;
            gun.OnScopeDisabled -= PlayerGun_OnScopeDisabled;
        }
        Melee melee = weapon.GetComponent<Melee>();
        if (melee != null)
        {

        }
    }
    private void SubscribeToWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            return;
        }
        weapon.OnShootingStarted += PlayerGun_OnShootingStarted;
        weapon.OnShootingEnd += PlayerGun_OnShootingEnd;
        Gun gun = weapon.GetComponent<Gun>();
        if (gun != null)
        {
            gun.OnReloadStart += PlayerGun_OnReloadStart;
            gun.OnReloadEnd += PlayerGun_OnReloadEnd;
            gun.OnReloadCancel += PlayerGun_OnReloadCancel;
            gun.OnScopeEnabled += PlayerGun_OnScopeEnabled;
            gun.OnScopeDisabled += PlayerGun_OnScopeDisabled;
        }
        Melee melee = weapon.GetComponent<Melee>();
        if (melee != null)
        {

        }
    }
}
