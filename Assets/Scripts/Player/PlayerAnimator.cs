
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
    private PlayerGun playerGun;
    // Start is called before the first frame update
    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        playerGun = GetComponent<PlayerGun>();
        playerGun.OnGunChanged += PlayerGun_OnGunChanged;
        playerGun.OnReloadStart += PlayerGun_OnReloadStart;
        playerGun.OnReloadEnd += PlayerGun_OnReloadEnd;
        playerGun.OnReloadCancel += PlayerGun_OnReloadCancel;
        playerGun.OnScopeEnabled += PlayerGun_OnScopeEnabled;
        playerGun.OnScopeDisabled += PlayerGun_OnScopeDisabled;
        playerGun.OnShootingStarted += PlayerGun_OnShootingStarted;
        playerGun.OnShootingEnd += PlayerGun_OnShootingEnd;
        playerGun.OnGunPulled += PlayerGun_OnGunPulled;


    }

    private void PlayerGun_OnGunPulled(object sender, System.EventArgs e)
    {
        ResetIdleAnimation();
    }

    private void PlayerGun_OnGunChanged(object sender,  PlayerGun.OnGunChangedEventArgs e)
    {
        animator.SetTrigger("ChangeGun");
        animator.SetTrigger(playerGun.GetGun().gameObject.name);
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
    //private void UnsubscribeFromGun(Gun gun)
    //{
    //    if(gun  != null)
    //    {
    //        gun.OnReloadStart -= PlayerGun_OnReloadStart;
    //        gun.OnReloadEnd -= PlayerGun_OnReloadEnd;
    //        gun.OnReloadCancel -= PlayerGun_OnReloadCancel;
    //        gun.OnScopeEnabled -= PlayerGun_OnScopeEnabled;
    //        gun.OnScopeDisabled -= PlayerGun_OnScopeDisabled;
    //        gun.OnShootingStarted -= PlayerGun_OnShootingStarted;
    //        gun.OnShootingEnd -= PlayerGun_OnShootingEnd;
    //    }
    //}
    //private void SubscribeToGun(Gun gun)
    //{
    //    if (gun != null)
    //    {
    //        gun.OnReloadStart += PlayerGun_OnReloadStart;
    //        gun.OnReloadEnd += PlayerGun_OnReloadEnd;
    //        gun.OnReloadCancel += PlayerGun_OnReloadCancel;
    //        gun.OnScopeEnabled += PlayerGun_OnScopeEnabled;
    //        gun.OnScopeDisabled += PlayerGun_OnScopeDisabled;
    //        gun.OnShootingStarted += PlayerGun_OnShootingStarted;
    //        gun.OnShootingEnd += PlayerGun_OnShootingEnd;
    //    }
    //}
}
