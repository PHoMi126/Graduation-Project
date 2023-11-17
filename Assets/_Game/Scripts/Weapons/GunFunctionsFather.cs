using System.Collections;
using TMPro;
using UnityEngine;

public class GunFunctionsFather : MonoBehaviour
{
    [Header("Animation")]
    public AnimStates animStates;
    public StaminaBar sprintFunc;

    [Header("S.Object")]
    public GunData gunData;

    [Header("RayCast Spawn")]
    public Transform cam;

    [Header("Ammo UI")]
    public Transform ammoCount;
    public TextMeshProUGUI currentAmmoInClip;
    public TextMeshProUGUI ammoReserve;

    [Header("Particle System")]
    public ParticleSystem muzzleFlash;

    protected float timeSinceLastShot;

    public LayerMask layerMask;
    protected RaycastHit hit;

    protected bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    protected void OnDisable() => gunData.reloading = false;

    public void AmmoUI()
    {
        currentAmmoInClip.text = gunData.currentAmmoInClip.ToString();
        ammoReserve.text = gunData.ammoReserve.ToString();
    }

    protected IEnumerator Reload()
    {
        var shot = gunData.magSize - gunData.currentAmmoInClip;
        if (gunData.currentAmmoInClip > gunData.magSize)
            yield break;

        if (gunData.ammoReserve > 0)
        {
            gunData.reloading = true;

            yield return new WaitForSeconds(gunData.reloadTime);

            gunData.ammoReserve -= shot;
            gunData.currentAmmoInClip += shot;

            gunData.reloading = false;
        }

        if (gunData.ammoReserve < 0)
        {
            gunData.ammoReserve = 0;
        }
    }
}
