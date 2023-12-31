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

    [Header("Sounds")]
    public AudioSource gunShotSound;
    public AudioSource reloadSound;

    [Header("LayerMask")]
    public LayerMask layerMask;

    protected float timeSinceLastShot;
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

        if (gunData.ammoReserve > 0 && gunData.currentAmmoInClip < gunData.magSize)
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

    protected void GunShotPhysic()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, gunData.maxDistance, layerMask))
        {
            IDamagable damagable = hit.transform.GetComponent<IDamagable>();
            damagable?.TakeDamage(gunData.damage);
        }
        gunData.currentAmmoInClip--;

        timeSinceLastShot = 0f;
        OnGunShot();
    }

    private void OnGunShot()
    {
        muzzleFlash.Play();
        gunShotSound.Play();
    }
}
