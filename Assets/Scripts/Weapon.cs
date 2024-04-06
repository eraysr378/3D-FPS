
using UnityEngine;
using System;
using Unity.VisualScripting;
public enum WeaponType
{
    Punch,
    TacticalRifle,
    Knife,
    Pistol

}
public class Weapon : MonoBehaviour
{
    public static AudioSource source;
    public static event EventHandler OnShootingStarted;
    public static event EventHandler OnShootingEnd;
    protected Animator animator;
    [SerializeField] protected WeaponType weaponType;
    [SerializeField] protected bool isShooting;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float pullTime; // for pull animation
    [SerializeField] private bool canShootAutomatic;
   
    
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void Shoot()
    {
    }
    public virtual void RightClickAction()
    {

    }
    public virtual void StopShooting()
    {

    }
    public virtual void ChangeWeapon()
    {

    }
    public void ForceStopShooting()
    {
        InvokeOnShootingEnd();
        isShooting = false;
    }
    public virtual float GetShootClipPitch()
    {
        return 1f;
    }
    public AudioClip GetShootClip()
    {
        return shootClip;
    }
    public float GetPullTime()
    {
        return pullTime;
    }
    public float GetTimeBetweenShots()
    {
        return timeBetweenShots;
    }
    protected void InvokeOnShootingEnd()
    {
        OnShootingEnd?.Invoke(this, EventArgs.Empty);
    }
    protected void InvokeOnShootingStarted()
    {
        OnShootingStarted?.Invoke(this, EventArgs.Empty);
    }
    public bool IsShooting()
    {
        return isShooting;
    }
    public WeaponType GetWeaponType()
    {
        return weaponType;
    }
    public bool CanShootAutomatic()
    {
        return canShootAutomatic;
    }
    public Animator GetAnimator()
    {
        return animator;
    }
}


