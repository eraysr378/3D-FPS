using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float magCapacity;
    [SerializeField] private float bulletsLeft;

    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float pullTime; // for pull animation

    [SerializeField] private float reloadTime;


    [SerializeField] private GameObject bulletHitPrefab;

    [Header("Recoil System")]
    [SerializeField] private Vector3 hipfireRecoil;
    [SerializeField] private Vector3 scopedRecoil;
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    [SerializeField] private float headshotDamage;
    [SerializeField] private float bodyshotDamage;
    [SerializeField] private float legshotDamage;



    private void Start()
    {
        bulletsLeft = magCapacity;

    }
    public float GetPullTime()
    {
        return pullTime;
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
    public float GetTimeBetweenShots()
    {
        return timeBetweenShots;
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
}
