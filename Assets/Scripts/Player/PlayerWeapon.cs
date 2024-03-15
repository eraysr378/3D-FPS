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


    private int currentWeaponIndex;
    private float pullTimer;

    // Start is called before the first frame update
    private void Awake()
    {
        recoil = FindObjectOfType<Recoil>();
        OnWeaponChanged += PlayerWeapon_OnWeaponChanged;

    }

    private void PlayerWeapon_OnWeaponChanged(object sender, OnWeaponChangedEventArgs e)
    {

        pullTimer = 0;
        isWeaponPulled = false;
        weapon.ChangeWeapon();
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
                currentWeaponIndex++;
                if (currentWeaponIndex >= weaponList.Count)
                {
                    currentWeaponIndex = 0;
                }
                weapon = weaponList[currentWeaponIndex];
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
        if (weapon == null)
        {
            return;
        }
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
    public void RightClickAction()
    {
        if (weapon != null && isWeaponPulled)
        {
            weapon.RightClickAction();
        }
    }
    public void Shoot()
    {
        if (weapon != null && isWeaponPulled)
        {
            weapon.Shoot();
        }
    }
    public void StopShooting()
    {
        if (weapon != null && isWeaponPulled)
        {
            weapon.StopShooting();
        }

    }

    public void AddWeapon(Weapon weapon)
    {
        if (!weaponList.Contains(weapon))
        {
            weaponList.Add(weapon);
        }
    }
    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }
    public Weapon GetWeapon()
    {
        return weapon;
    }


    public Camera GetCamera()
    {
        return cam;
    }
    public Recoil GetRecoil()
    {
        return recoil;

    }
    public bool IsShooting()
    {
        if (weapon == null)
        {
            return false;
        }
        return weapon.IsShooting();
    }
}
