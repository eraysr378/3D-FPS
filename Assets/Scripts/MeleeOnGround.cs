using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeOnGround : Interactable
{
    [SerializeField] private Melee melee;
    [SerializeField] private PlayerWeapon playerWeapon;
    public override void Interact()
    {
        base.Interact();
        Debug.Log(gameObject.name + " interact");
        playerWeapon.AddWeapon(melee);
        Destroy(gameObject);
    }
}
