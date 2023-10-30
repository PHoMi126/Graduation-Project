using System.Collections;
using TMPro;
using UnityEngine;

public class Revolver : MonoBehaviour
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
    public TextMeshProUGUI currentAmmoInClip;
    public TextMeshProUGUI ammoReserve;

    float timeSinceLastShot;

    public LayerMask layerMask;
    RaycastHit hit;

    private void Start()
    {
        AmmoUI();
        animStates.ChangeAnim(AnimStates.AnimState.revolverIdle);

        //Subcribe to events
        WeaponActions.AttackInput += M44Shoot;
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

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void AmmoUI()
    {
        currentAmmoInClip.text = gunData.currentAmmoInClip.ToString();
        ammoReserve.text = gunData.ammoReserve.ToString();
    }

    public void M44Shoot()
    {
        if (gunData.currentAmmoInClip > 0)
        {
            if (CanShoot() && this.gameObject.activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.revolverShoot);

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

    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReloadRevolver()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
            animStates.ChangeAnim(AnimStates.AnimState.revolverReload);
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmoInClip = gunData.magSize;

        gunData.reloading = false;
    }

    private void RevolverSprint()
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
