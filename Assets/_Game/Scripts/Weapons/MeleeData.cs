using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/Melee", fileName = "MeleeData")]
public class MeleeData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Functions")]
    public float damage;
    public float fireRate;
    public float maxDistance;
}
