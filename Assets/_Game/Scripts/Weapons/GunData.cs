using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/Guns", fileName = "GunData")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Ammo Relate")]
    public int currentAmmoInClip;
    public int magSize;
    public int ammoReserve;
    public int capacity;

    [Header("Functions")]
    public float damage;
    public float fireRate;
    public float reloadTime;
    public float maxDistance;
    internal bool reloading;
}
