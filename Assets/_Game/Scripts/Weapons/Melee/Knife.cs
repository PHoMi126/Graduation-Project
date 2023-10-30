using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] AnimStates animStates;
    [SerializeField] StaminaBar sprintFunc;

    [Header("S.Object")]
    [SerializeField] GunData gunData;

    [Header("RayCast Spawn")]
    [SerializeField] Transform cam;

    [Header("Ammo UI")]
    public Transform ammoCount;

    float timeSinceLastSwing;

    public LayerMask layerMask;
    RaycastHit hit;
}
