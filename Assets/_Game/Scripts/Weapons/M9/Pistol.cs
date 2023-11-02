using System.Collections;
using TMPro;
using UnityEngine;

public class Pistol : MonoBehaviour
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
        animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);

        //Subcribe to events
        WeaponActions.AttackInput += M9Shoot;
        WeaponActions.reloadInput += StartReloadPistol;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward);
        AmmoUI();
        PistolSprint();
        ammoCount.gameObject.SetActive(true);
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void AmmoUI()
    {
        currentAmmoInClip.text = gunData.currentAmmoInClip.ToString();
        ammoReserve.text = gunData.ammoReserve.ToString();
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

    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReloadPistol()
    {
        if (!gunData.reloading && gameObject.activeSelf /* Stop reload if inactive*/)
        {
            StartCoroutine(Reload());
            animStates.ChangeAnim(AnimStates.AnimState.pistolReload);
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmoInClip = gunData.magSize;

        gunData.reloading = false;
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
