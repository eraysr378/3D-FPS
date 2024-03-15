using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOnGround : Interactable
{
    [SerializeField] private Gun gun;
    [SerializeField] private PlayerWeapon playerWeapon;
    public override void Interact()
    {
       base.Interact();
        Debug.Log(gameObject.name + " interact");
        gun.SetCamera(playerWeapon.GetCamera());
        gun.SetRecoil(playerWeapon.GetRecoil());
        playerWeapon.AddWeapon(gun);
        Destroy(gameObject);
    }
}
