using UnityEngine;

public class Pistol : GunFunctionsFather
{
    private void Start()
    {
        AmmoUI();
        animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
        gunData.currentAmmoInClip = gunData.magSize;
        gunData.ammoReserve = gunData.capacity - gunData.magSize;

        //Subcribe to events
        WeaponActions.AttackInput += M9Shoot;
        WeaponActions.reloadInput += StartReloadPistol;
    }

    private void OnDestroy()
    {
        WeaponActions.AttackInput -= M9Shoot;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward);
        AmmoUI();
        PistolSprint();
        ammoCount.gameObject.SetActive(true);
    }

    public void M9Shoot()
    {
        if (gunData.currentAmmoInClip > 0)
        {
            if (CanShoot() && this.gameObject.activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.pistolShoot);

                if (Physics.Raycast(cam.position, cam.forward, out hit, gunData.maxDistance, layerMask))
                {
                    IDamagable damagable = hit.transform.GetComponent<IDamagable>();
                    damagable?.TakeDamage(gunData.damage);
                }

                gunData.currentAmmoInClip--;
                timeSinceLastShot = 0f;
                OnGunShot();
            }
        }
    }

    private void OnGunShot()
    {
        muzzleFlash.Play();
        gunShotSound.Play();
    }

    public void StartReloadPistol()
    {
        if (!gunData.reloading && gameObject.activeSelf /* Stop reload if inactive*/)
        {
            StartCoroutine(Reload());
            animStates.ChangeAnim(AnimStates.AnimState.pistolReload);
            reloadSound.Play();
        }
    }

    private void PistolSprint()
    {
        if (gameObject.activeSelf && !gunData.reloading)
        {
            if (sprintFunc.isSprinting == true)
            {
                animStates.ChangeAnim(AnimStates.AnimState.pistolSprint);
            }
            else
            {
                animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
            }
        }
    }
}
