using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data", order = 1, fileName = "Weapon Datas")]
public class WeaponData : ScriptableObject
{
    public float ammoInClip;
    public float ammoReserve;
    public float ammoMaxCarry;

    public float damage;
}
