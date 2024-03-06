using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
    [SerializeField] private AudioSource weaponAudioSource;
    private void Start()
    {
        Weapon.OnShootingStarted += Weapon_OnShootingStarted;
    }

    private void Weapon_OnShootingStarted(object sender, System.EventArgs e)
    {
        Weapon weapon = sender as Weapon;
        weaponAudioSource.clip = weapon.GetShootClip();
        weaponAudioSource.volume = 0.1f;
        weaponAudioSource.pitch = weapon.GetShootClipPitch();
        weaponAudioSource.Play();
        //PlaySound(weapon.GetShootClip(),weapon.transform.position,0.1f);
    }

    public static void PlaySound(AudioClip audioClip,Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip,position,volume);
    }
}
