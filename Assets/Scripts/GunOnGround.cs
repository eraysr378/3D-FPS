using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOnGround : Interactable
{
    [SerializeField] private Gun gun;
    [SerializeField] private PlayerGun playerGun;
    public override void Interact()
    {
       base.Interact();
        Debug.Log(gameObject.name + " interact");
        playerGun.AddGun(gun);
    }
}
