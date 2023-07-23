using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gun",menuName ="Weapon/gun")]
public class GunData : ScriptableObject
{
    [Header ("Info")]
    public new string name;

    [Header("Shooting")]
    public int damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;

    [HideInInspector]
    public bool reloading;

}
