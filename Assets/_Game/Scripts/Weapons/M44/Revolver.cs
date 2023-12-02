using UnityEngine;

public class Revolver : GunFunctionsFather
{
    private void Start()
    {
        AmmoUI();
        animStates.ChangeAnim(AnimStates.AnimState.revolverIdle);
        gunData.currentAmmoInClip = gunData.magSize;
        gunData.ammoReserve = gunData.capacity - gunData.magSize;

        //Subcribe to events
        WeaponActions.attackInput += M44Shoot;
        WeaponActions.reloadInput += StartReloadRevolver;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward);
        AmmoUI();
        RevolverSprint();
        ammoCount.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        WeaponActions.attackInput -= M44Shoot;
        WeaponActions.reloadInput -= StartReloadRevolver;
    }

    public void M44Shoot()
    {
        if (gunData.currentAmmoInClip > 0)
        {
            if (CanShoot() && this.gameObject.activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.revolverShoot);

                GunShotPhysic();
            }
        }
    }



    public void StartReloadRevolver()
    {
        if (!gunData.reloading && gameObject.activeSelf)
        {
            StartCoroutine(Reload());
            animStates.ChangeAnim(AnimStates.AnimState.revolverReload);
            reloadSound.Play();
        }
    }

    private void RevolverSprint()
    {
        if (gameObject.activeSelf && !gunData.reloading)
        {
            if (sprintFunc.isSprinting == true)
            {
                animStates.ChangeAnim(AnimStates.AnimState.revolverSprint);
            }
            else
            {
                animStates.ChangeAnim(AnimStates.AnimState.revolverIdle);
            }
        }
    }
}
