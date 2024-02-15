using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public event EventHandler OnReloadStart;
    public event EventHandler OnReloadCancel;
    public event EventHandler OnReloadEnd;
    public event EventHandler OnScopeEnabled;
    public event EventHandler OnScopeDisabled;
    public event EventHandler OnShootingStarted;
    public event EventHandler OnShootingEnd;
    public event EventHandler OnGunPulled;
    public event EventHandler<OnGunChangedEventArgs> OnGunChanged;
    public class OnGunChangedEventArgs : EventArgs
    {
        public Gun previousGun;
    }
    [SerializeField] private Gun gun;
    [SerializeField] private List<Gun> gunList = new List<Gun>();


    [SerializeField] private bool canShoot;
    [SerializeField] private bool isReloading;
    [SerializeField] private bool isGunPulled;

    [SerializeField] private bool isShooting;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private bool isScopeEnabled;

    [Header("Recoil System")]
    [SerializeField] private Recoil recoil;

    [SerializeField] private Transform damageTextPosition;
    [SerializeField] private TextMeshProUGUI headshotText;
    [SerializeField] private TextMeshProUGUI bodyshotText;
    [SerializeField] private TextMeshProUGUI legshotText;
    private float reloadTimer;
    private float shootTimer;
    private float pullTimer;

    // Start is called before the first frame update
    private void Awake()
    {
        recoil = FindObjectOfType<Recoil>();
        OnGunChanged += PlayerGun_OnGunChanged;

    }

    private void PlayerGun_OnGunChanged(object sender, OnGunChangedEventArgs e)
    {
        
        pullTimer = 0;
        isGunPulled = false;
        CancelReloading();
        DisableScope();
        ForceStopShooting();
        canShoot = true;
    }

    void Start()
    {

        if (gun != null)
        {
            canShoot = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Gun prevGun = gun;
            if (gunList.Count > 0)
            {
                gun = gunList[UnityEngine.Random.Range(0, gunList.Count)];
                gun.gameObject.SetActive(true);
                OnGunChanged?.Invoke(this, new OnGunChangedEventArgs { previousGun = prevGun });

            }
            if (prevGun != null && gun != prevGun)
            {
                prevGun.gameObject.SetActive(false);
            }

        }
        if (gun == null)
        {
            return;
        }
        pullTimer += Time.deltaTime;
        if(pullTimer > gun.GetPullTime() && !isGunPulled)
        {
            isGunPulled = true;
            OnGunPulled?.Invoke(this, EventArgs.Empty);
        }
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer > gun.GetReloadTime())
            {
                EndReloading();
            }
        }
        if (shootTimer < gun.GetTimeBetweenShots())
        {
            shootTimer += Time.deltaTime;
        }



    }
    
    public void Reload()
    {
        StartReloading();
    }
    public void StartReloading()
    {
        if (gun.GetBulletsLeft() == gun.GetMagCapacity() || isReloading)
        {
            return;
        }
        OnShootingEnd?.Invoke(this, EventArgs.Empty);
        reloadTimer = 0;
        isReloading = true;
        DisableScope();
        OnReloadStart?.Invoke(this, EventArgs.Empty);
    }
    private void EndReloading()
    {
        gun.SetBulletsLeft(gun.GetMagCapacity());
        isReloading = false;
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }
    public void CancelReloading()
    {
        isReloading = false;
        OnReloadCancel?.Invoke(this, EventArgs.Empty);
    }


    public void EnableDisableScope()
    {
        if (gun == null || isShooting || isReloading || !isGunPulled)
        {
            return;
        }
        if (isScopeEnabled)
        {
            DisableScope();
        }
        else
        {
            EnableScope();
        }

    }
    public void EnableScope()
    {
        isScopeEnabled = true;
        OnScopeEnabled?.Invoke(this, EventArgs.Empty);

    }
    public void DisableScope()
    {
        isScopeEnabled = false;
        OnScopeDisabled?.Invoke(this, EventArgs.Empty);
    }
    public void Shoot()
    {
        if (gun.GetBulletsLeft() <= 0)
        {
            Debug.Log("bullets left = 0;");
            StopShooting();
            return;
        }
        if (!canShoot || shootTimer < gun.GetTimeBetweenShots() || isReloading ||!isGunPulled)
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
                Debug.Log("enemy hit");

                TextMeshProUGUI hitDamageText;
                switch (LayerMask.LayerToName((rayHit.collider.gameObject.layer)))
                {
                    case "Head":
                        enemy.TakeDamage(gun.GetHeadshotDamage());
                        hitDamageText = Instantiate(headshotText, damageTextPosition);
                        hitDamageText.text = gun.GetHeadshotDamage().ToString();
                        break;
                    case "Leg":
                        enemy.TakeDamage(gun.GetLegshotDamage());

                        hitDamageText = Instantiate(legshotText, damageTextPosition);
                        hitDamageText.text = gun.GetLegshotDamage().ToString();
                        break;
                    // non-leg and non-head objects will take the bodyshotDamage;
                    default: 
                        enemy.TakeDamage(gun.GetBodyshotDamage());
                        hitDamageText = Instantiate(bodyshotText, damageTextPosition);
                        hitDamageText.text = gun.GetBodyshotDamage().ToString();
                        break;
                }
                hitDamageText.gameObject.SetActive(true);

            }


            Debug.Log("hit:" + rayHit.collider.gameObject.name);
            Instantiate(gun.GetBulletHitPrefab(), rayHit.point, Quaternion.identity);
        }

        shootTimer = 0;
        gun.SetBulletsLeft(gun.GetBulletsLeft() - 1);
        recoil.RecoilFire();
        isShooting = true;
        OnShootingStarted?.Invoke(this, EventArgs.Empty);

    }
   
    public bool IsScopeEnabled()
    {
        return isScopeEnabled;
    }
    public void StopShooting()
    {
        // this if added to fix animation when a single shot fired
        if (shootTimer >= gun.GetTimeBetweenShots() && isShooting)
        {
            OnShootingEnd?.Invoke(this, EventArgs.Empty);
            isShooting = false;
        }

    }
    public void ForceStopShooting()
    {
        OnShootingEnd?.Invoke(this, EventArgs.Empty);
        isShooting = false;
    }
    public bool IsShooting()
    {
        return isShooting;
    }
 

    public Gun GetGun()
    {
        return gun;
    }
    public void SetGun(Gun newGun)
    {
        gun = newGun;
    }
    public void AddGun(Gun gun)
    {
        if (!gunList.Contains(gun))
        {
            gunList.Add(gun);
        }
    }
}
