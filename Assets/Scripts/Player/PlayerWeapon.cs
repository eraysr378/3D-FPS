using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public event EventHandler OnWeaponPulled;
    public event EventHandler<OnWeaponChangedEventArgs> OnWeaponChanged;
    public class OnWeaponChangedEventArgs : EventArgs
    {
        public Weapon previousWeapon;
    }
    [SerializeField] private Weapon weapon;
    [SerializeField] private List<Weapon> weaponList = new List<Weapon>();


    [SerializeField] private bool isWeaponPulled;

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask hitLayerMask;

    [Header("Recoil System")]
    [SerializeField] private Recoil recoil;



    private float pullTimer;

    // Start is called before the first frame update
    private void Awake()
    {
        recoil = FindObjectOfType<Recoil>();
        OnWeaponChanged += PlayerWeapon_OnWeaponChanged;

    }

    private void PlayerWeapon_OnWeaponChanged(object sender, OnWeaponChangedEventArgs e)
    {
        Gun gun = weapon.GetComponent<Gun>();
        if (gun != null)
        {
            gun.CancelReloading();
            gun.DisableScope();
            gun.ForceStopShooting();
        }
        pullTimer = 0;
        isWeaponPulled = false;
    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Weapon prevWeapon = weapon;
            if (weaponList.Count > 0)
            {
                weapon = weaponList[UnityEngine.Random.Range(0, weaponList.Count)];
                weapon.gameObject.SetActive(true);
                OnWeaponChanged?.Invoke(this, new OnWeaponChangedEventArgs { previousWeapon = prevWeapon });

            }
            if (prevWeapon != null && weapon != prevWeapon)
            {
                prevWeapon.gameObject.SetActive(false);
            }

        }
        if (weapon == null)
        {
            return;
        }
        pullTimer += Time.deltaTime;
        if (pullTimer > weapon.GetPullTime() && !isWeaponPulled)
        {
            isWeaponPulled = true;
            OnWeaponPulled?.Invoke(this, EventArgs.Empty);
        }




    }

    public void Reload()
    {
        Gun gun = weapon.GetComponent<Gun>();
        if (gun != null)
        {
            gun.StartReloading();
        }
    }
    public void EnableDisableScope()
    {

        Gun gun = weapon.GetComponent<Gun>();
        if (gun == null || !isWeaponPulled)
        {
            return;
        }
        if (gun.IsScopeEnabled())
        {
            gun.DisableScope();
        }
        else
        {
            gun.EnableScope();
        }
    }
    public void Shoot()
    {
        Gun gun = weapon.GetComponent<Gun>();
        if (gun != null && isWeaponPulled)
        {
            gun.Shoot();
        }
    }
    public void StopShooting()
    {
        Gun gun = weapon.GetComponent<Gun>();
        if (gun != null && isWeaponPulled)
        {
            gun.StopShooting();
        }
    }

    //public void StartReloading()
    //{
    //    if (gun.GetBulletsLeft() == gun.GetMagCapacity() || isReloading)
    //    {
    //        return;
    //    }
    //    OnShootingEnd?.Invoke(this, EventArgs.Empty);
    //    reloadTimer = 0;
    //    isReloading = true;
    //    DisableScope();
    //    OnReloadStart?.Invoke(this, EventArgs.Empty);
    //}
    //private void EndReloading()
    //{
    //    gun.SetBulletsLeft(gun.GetMagCapacity());
    //    isReloading = false;
    //    OnReloadEnd?.Invoke(this, EventArgs.Empty);
    //}
    //public void CancelReloading()
    //{
    //    isReloading = false;
    //    OnReloadCancel?.Invoke(this, EventArgs.Empty);
    //}


    //public void EnableDisableScope()
    //{
    //    if (gun == null || isShooting || isReloading || !isGunPulled)
    //    {
    //        return;
    //    }
    //    if (isScopeEnabled)
    //    {
    //        DisableScope();
    //    }
    //    else
    //    {
    //        EnableScope();
    //    }

    //}
    //public void EnableScope()
    //{
    //    isScopeEnabled = true;
    //    OnScopeEnabled?.Invoke(this, EventArgs.Empty);

    //}
    //public void DisableScope()
    //{
    //    isScopeEnabled = false;
    //    OnScopeDisabled?.Invoke(this, EventArgs.Empty);
    //}
    //public void Shoot()
    //{
    //    if (gun.GetBulletsLeft() <= 0)
    //    {
    //        Debug.Log("bullets left = 0;");
    //        StopShooting();
    //        return;
    //    }
    //    if (!canShoot || shootTimer < gun.GetTimeBetweenShots() || isReloading ||!isGunPulled)
    //    {
    //        return;
    //    }
    //    RaycastHit rayHit;
    //    Vector3 direction = cam.transform.forward;
    //    if (Physics.Raycast(cam.transform.position, direction, out rayHit, 35f, hitLayerMask))
    //    {
    //        Enemy enemy = rayHit.collider.gameObject.GetComponentInParent<Enemy>();
    //        if (enemy != null)
    //        {
    //            Debug.Log("enemy hit");

    //            TextMeshProUGUI hitDamageText;
    //            switch (LayerMask.LayerToName((rayHit.collider.gameObject.layer)))
    //            {
    //                case "Head":
    //                    enemy.TakeDamage(gun.GetHeadshotDamage());
    //                    hitDamageText = Instantiate(headshotText, damageTextPosition);
    //                    hitDamageText.text = gun.GetHeadshotDamage().ToString();
    //                    break;
    //                case "Leg":
    //                    enemy.TakeDamage(gun.GetLegshotDamage());

    //                    hitDamageText = Instantiate(legshotText, damageTextPosition);
    //                    hitDamageText.text = gun.GetLegshotDamage().ToString();
    //                    break;
    //                // non-leg and non-head objects will take the bodyshotDamage;
    //                default: 
    //                    enemy.TakeDamage(gun.GetBodyshotDamage());
    //                    hitDamageText = Instantiate(bodyshotText, damageTextPosition);
    //                    hitDamageText.text = gun.GetBodyshotDamage().ToString();
    //                    break;
    //            }
    //            hitDamageText.gameObject.SetActive(true);

    //        }


    //        Debug.Log("hit:" + rayHit.collider.gameObject.name);
    //        Instantiate(gun.GetBulletHitPrefab(), rayHit.point, Quaternion.identity);
    //    }

    //    shootTimer = 0;
    //    gun.SetBulletsLeft(gun.GetBulletsLeft() - 1);
    //    recoil.RecoilFire();
    //    isShooting = true;
    //    OnShootingStarted?.Invoke(this, EventArgs.Empty);

    //}

    //public bool IsScopeEnabled()
    //{
    //    return isScopeEnabled;
    //}
    //public void StopShooting()
    //{
    //    // this if added to fix animation when a single shot fired
    //    if (shootTimer >= gun.GetTimeBetweenShots() && isShooting)
    //    {
    //        OnShootingEnd?.Invoke(this, EventArgs.Empty);
    //        isShooting = false;
    //    }

    //}
    //public void ForceStopShooting()
    //{
    //    OnShootingEnd?.Invoke(this, EventArgs.Empty);
    //    isShooting = false;
    //}
    //public bool IsShooting()
    //{
    //    return isShooting;
    //}


    public Weapon GetWeapon()
    {
        return weapon;
    }
    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }
    public void AddWeapon(Weapon weapon)
    {
        if (!weaponList.Contains(weapon))
        {
            weaponList.Add(weapon);
        }
    }
    public Camera GetCamera()
    {
        return cam;
    }
    public Recoil GetRecoil()
    {
        return recoil;

    }
}
