
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    public event EventHandler OnShootingStarted;
    public event EventHandler OnShootingEnd;
    [SerializeField] protected bool isShooting;

    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float pullTime; // for pull animation
   
    
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
    public virtual void StopShooting()
    {
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
}
