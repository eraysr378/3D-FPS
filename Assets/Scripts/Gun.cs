using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Gun : Weapon
{
    public static event EventHandler OnReloadStart;
    public static event EventHandler OnReloadCancel;
    public static event EventHandler OnReloadEnd;
    public static event EventHandler OnScopeEnabled;
    public static event EventHandler OnScopeDisabled;

    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private float magCapacity;
    [SerializeField] private float bulletsLeft;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool isReloading;

    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private bool isScopeEnabled;
    [SerializeField] private GameObject bulletHitPrefab;


    [Header("Recoil System")]
    [SerializeField] private Vector3 hipfireRecoil;
    [SerializeField] private Vector3 scopedRecoil;
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    [SerializeField] private float headshotDamage;
    [SerializeField] private float bodyshotDamage;
    [SerializeField] private float legshotDamage;
    private Animator animator;
    private Recoil recoil;
    private Camera cam;


    private float reloadTimer;
    private float shootTimer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        bulletsLeft = magCapacity;

    }
    private void Update()
    {
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer > GetReloadTime())
            {
                EndReloading();
            }
        }
        if (shootTimer < GetTimeBetweenShots())
        {
            shootTimer += Time.deltaTime;
        }
    }
    public void StartReloading()
    {
        if (GetBulletsLeft() == GetMagCapacity() || isReloading)
        {
            return;
        }
        InvokeOnShootingEnd();
        reloadTimer = 0;
        isReloading = true;
        DisableScope();
        if (animator != null)
        {
            animator.SetTrigger("ReloadTrigger");
            animator.SetBool("Reload", true);

        }
        OnReloadStart?.Invoke(this, EventArgs.Empty);
    }
    private void EndReloading()
    {
        SetBulletsLeft(GetMagCapacity());
        isReloading = false;
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
        if (animator != null)
        {
            animator.SetBool("Reload", false);

        }
    }
    public void CancelReloading()
    {
        isReloading = false;
        OnReloadCancel?.Invoke(this, EventArgs.Empty);
        if (animator != null)
        {
            animator.SetBool("Reload", false);

        }
    }


    public override void ChangeWeapon()
    {
        CancelReloading();
        DisableScope();
        ForceStopShooting();
    }
    public void EnableScope()
    {
        if (!isReloading && !isShooting)
        {
            isScopeEnabled = true;
            OnScopeEnabled?.Invoke(this, EventArgs.Empty);
        }
    }
    public void DisableScope()
    {
        isScopeEnabled = false;
        OnScopeDisabled?.Invoke(this, EventArgs.Empty);
    }
    public override void RightClickAction()
    {
        if (isScopeEnabled)
        {
            DisableScope();
        }
        else
        {
            EnableScope();
        }
    }
    public override void Shoot()
    {
        if (GetBulletsLeft() <= 0)
        {
            Debug.Log("bullets left = 0;");
            StopShooting();
            return;
        }
        if (shootTimer < GetTimeBetweenShots() || isReloading)
        {
            return;
        }
        RaycastHit rayHit;
        Vector3 direction = cam.transform.forward;
        if (Physics.Raycast(cam.transform.position, direction, out rayHit, 35f, hitLayerMask))
        {

            Enemy enemy = rayHit.collider.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                switch (LayerMask.LayerToName((rayHit.collider.gameObject.layer)))
                {
                    case "Head":
                        enemy.TakeDamage(headshotDamage);
                        HitDamageTextSpawner.Instance.SpawnHeadshotText(headshotDamage.ToString());
                        break;
                    case "Leg":
                        enemy.TakeDamage(legshotDamage);
                        HitDamageTextSpawner.Instance.SpawnLegshotText(legshotDamage.ToString());
                        break;
                    // non-leg and non-head objects will take the bodyshotDamage;
                    default:
                        enemy.TakeDamage(bodyshotDamage);
                        HitDamageTextSpawner.Instance.SpawnBodyshotText(bodyshotDamage.ToString());

                        break;
                }
            }
            CreateBulletImpactEffect(rayHit);
        }

        shootTimer = 0;
        SetBulletsLeft(GetBulletsLeft() - 1);
        recoil.RecoilFire();
        isShooting = true;
        muzzlePrefab.SetActive(false); // if activated previously and not disabled yet, make sure it is disabled.
        muzzlePrefab.SetActive(true);
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

    public bool IsScopeEnabled()
    {
        return isScopeEnabled;
    }


    public float GetBulletsLeft()
    {
        return bulletsLeft;
    }
    public void SetBulletsLeft(float newBulletsLeft)
    {
        bulletsLeft = newBulletsLeft;
    }
    public Vector3 GetHipfireRecoil()
    {
        return hipfireRecoil;
    }
    public Vector3 GetScopedRecoil()
    {
        return scopedRecoil;
    }
    public float GetSnappiness()
    {
        return snappiness;
    }
    public float GetReturnSpeed()
    {
        return returnSpeed;
    }
    public float GetMagCapacity()
    {
        return magCapacity;
    }
    public float GetReloadTime()
    {
        return reloadTime;
    }

    public float GetHeadshotDamage()
    {
        return headshotDamage;
    }
    public float GetBodyshotDamage()
    {
        return bodyshotDamage;
    }
    public float GetLegshotDamage()
    {
        return legshotDamage;
    }
    public GameObject GetBulletHitPrefab()
    {
        return bulletHitPrefab;
    }
    public void SetRecoil(Recoil recoil)
    {
        this.recoil = recoil;
    }
    public void SetCamera(Camera camera)
    {
        cam = camera;
    }
    public override float GetShootClipPitch()
    {
        float pitch = UnityEngine.Random.Range(1.2f, 1.4f);
        //pitch += Mathf.Sqrt(consecutiveShotCount / 30);
        Debug.Log(pitch);
        return pitch;
    }
    private void CreateBulletImpactEffect(RaycastHit rayHit)
    {

        GameObject hole = Instantiate(bulletHitPrefab, rayHit.point, Quaternion.identity);

    }
}
