using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/Guns", fileName = "GunData")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Ammo Relate")]
    public float currentAmmoInClip;
    public float magSize;
    public float ammoReserve;

    [Header("Functions")]
    public float damage;
    public float fireRate;
    public float reloadTime;
    public float maxDistance;
    internal bool reloading;
}
