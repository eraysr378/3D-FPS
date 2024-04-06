
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string RELOAD = "Reload";
    private const string ENABLE_SCOPE_FLOAT = "EnableScopeFloat";
    private const string ENABLE_SCOPE = "EnableScope";
    private const string IS_SHOOTING = "IsShooting";
    private const string SHOOT_TRIGGER = "ShootTrigger";
    private const string SECONDARY_ATTACK_TRIGGER = "SecondaryAttackTrigger";

    private const string RELOAD_TRIGGER = "ReloadTrigger";
    private const string CHANGE_WEAPON = "ChangeWeapon";
    private const string WEAPON_TYPE = "WeaponType";
    private const string IS_SECONDARY_ATTACKING = "IsSecondaryAttacking";

    private const string X = "x";
    private const string Y = "y";
    [SerializeField] private Animator animator;
    private Animator weaponAnimator;
    private PlayerMotor playerMotor;
    private PlayerWeapon playerWeapon;
    private void Awake()
    {
        playerMotor = GetComponent<PlayerMotor>();
        playerWeapon = GetComponent<PlayerWeapon>();

    }
    private void Start()
    {
        playerWeapon.OnWeaponChanged += PlayerWeapon_OnWeaponChanged;
        playerWeapon.OnWeaponPulled += PlayerWeapon_OnWeaponPulled;
        Weapon.OnShootingStarted += Weapon_OnShootingStarted;
        Weapon.OnShootingEnd += Weapon_OnShootingEnd;
        Gun.OnScopeEnabled += Gun_OnScopeEnabled;
        Gun.OnScopeDisabled += Gun_OnScopeDisabled;
        Gun.OnReloadStart += Gun_OnReloadStart;
        Gun.OnReloadEnd += Gun_OnReloadEnd;
        Gun.OnReloadCancel += Gun_OnReloadCancel;
        Melee.OnSecondaryAttackStarted += Melee_OnSecondaryAttackStarted;
        Melee.OnSecondaryAttackEnd += Melee_OnSecondaryAttackEnd;
    }

    private void Melee_OnSecondaryAttackEnd(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_SECONDARY_ATTACKING, false);
        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
    }

    private void Melee_OnSecondaryAttackStarted(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_SECONDARY_ATTACKING, true);
        animator.SetTrigger(SECONDARY_ATTACK_TRIGGER);
    }


    private void Gun_OnReloadCancel(object sender, System.EventArgs e)
    {
        animator.SetBool(RELOAD, false);
        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
    }

    private void Gun_OnReloadEnd(object sender, System.EventArgs e)
    {
        animator.SetBool(RELOAD, false);

        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
    }

    private void Gun_OnReloadStart(object sender, System.EventArgs e)
    {
        animator.SetBool(RELOAD, true);
        animator.SetTrigger(RELOAD_TRIGGER);
    }

    private void Gun_OnScopeDisabled(object sender, System.EventArgs e)
    {
        animator.SetBool(ENABLE_SCOPE, false);
    }

    private void Gun_OnScopeEnabled(object sender, System.EventArgs e)
    {
        animator.SetBool(ENABLE_SCOPE, true);
    }

    private void Weapon_OnShootingEnd(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_SHOOTING, false);
        if (weaponAnimator != null)
        {
            weaponAnimator.SetBool(IS_SHOOTING, false);

        }

        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
    }

    private void Weapon_OnShootingStarted(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_SHOOTING, true);
        animator.SetTrigger(SHOOT_TRIGGER);
        if (weaponAnimator != null)
        {
            weaponAnimator.SetBool(IS_SHOOTING, true);
            weaponAnimator.SetTrigger(SHOOT_TRIGGER);
        }
        else
        {
            Debug.Log("animator null");
        }


    }


    private void PlayerWeapon_OnWeaponPulled(object sender, System.EventArgs e)
    {
        ResetIdleAnimation();
    }

    private void PlayerWeapon_OnWeaponChanged(object sender, PlayerWeapon.OnWeaponChangedEventArgs e)
    {
        animator.SetInteger(WEAPON_TYPE, (int)playerWeapon.GetWeapon().GetWeaponType());
        animator.SetTrigger(CHANGE_WEAPON);
        weaponAnimator = playerWeapon.GetWeapon().GetAnimator();

        // make idle the current animation so that animation blend tree works
        ResetIdleAnimation();
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


}
