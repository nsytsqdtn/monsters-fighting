using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour {
    public AudioSource OpenGunSound;
    public AudioSource OpenMissileSound;
    public AudioSource missile;
    public void OpenMissile()
    {
        OpenMissileSound.Play();
    }
    public void OpenGun()
    {
        OpenGunSound.Play();
    }
    public void OpenBoom()
    {
        missile.Play();
    }
}
