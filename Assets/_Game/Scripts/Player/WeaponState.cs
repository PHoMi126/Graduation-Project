using System;
using System.Collections;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] AnimStates animStates;
    [SerializeField] KeyCode reload;
    [SerializeField] KeyCode sprint;

    [Header("Weapon Active")]
    [SerializeField] GameObject[] weapons;
    [SerializeField] KeyCode meleeSlot;
    [SerializeField] KeyCode pistolSlot;
    [SerializeField] KeyCode revolverSlot;

    [Header("Sound")]
    [SerializeField] AudioClip[] clips;
    [SerializeField] float volume = 1f;

    [Header("Ammo UI")]
    [SerializeField] MonoBehaviour[] weaponsUI;

    bool isSprinting;
    bool canSprint;
    bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        animStates.ChangeAnim(AnimStates.AnimState.meleeIdle);
        AudioSource.PlayClipAtPoint(clips[0], transform.position, volume);
        weapons[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopAttack();
        }

        if (canSprint == true)
        {
            Sprint();
        }

        Reload();
        ChangeWeapon();
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(meleeSlot) && weapons[0] != null)
        {
            animStates.ChangeAnim(AnimStates.AnimState.switchWeapon);
            animStates.animator.SetInteger("weaponType", 1);

            //lambda
            StartCoroutine(IEDelayAction(
                () =>
                {
                    weapons[0].SetActive(true);
                    weapons[1].SetActive(false);
                    weapons[2].SetActive(false);
                }, 0.5f));

            animStates.ChangeAnim(AnimStates.AnimState.meleeIdle);
        }

        if (Input.GetKeyDown(pistolSlot) && weapons[1] != null)
        {
            animStates.ChangeAnim(AnimStates.AnimState.switchWeapon);
            animStates.animator.SetInteger("weaponType", 2);

            //lambda
            StartCoroutine(IEDelayAction(
                () =>
                {
                    weapons[0].SetActive(false);
                    weapons[1].SetActive(true);
                    weapons[2].SetActive(false);
                }, 0.5f));

            animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
        }

        if (Input.GetKeyDown(revolverSlot) && weapons[2] != null)
        {
            animStates.ChangeAnim(AnimStates.AnimState.switchWeapon);
            animStates.animator.SetInteger("weaponType", 4);

            StartCoroutine(IEDelayAction(
                () =>
                {
                    weapons[0].SetActive(false);
                    weapons[1].SetActive(false);
                    weapons[2].SetActive(true);
                }, 0.5f));

            animStates.ChangeAnim(AnimStates.AnimState.revolverIdle);
        }
    }

    private IEnumerator IEDelayAction(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    private void Attack()
    {
        canSprint = false;
        if (weapons[0].activeSelf)
        {
            animStates.ChangeAnim(AnimStates.AnimState.meleeAtk);
        }

        if (weapons[1].activeSelf)
        {
            animStates.ChangeAnim(AnimStates.AnimState.pistolShoot);
            AudioSource.PlayClipAtPoint(clips[1], transform.position, volume);
        }

        if (weapons[2].activeSelf)
        {
            animStates.ChangeAnim(AnimStates.AnimState.revolverShoot);
        }
    }

    private void StopAttack()
    {
        canSprint = true;
        if (weapons[0].activeSelf)
        {
            animStates.ChangeAnim(AnimStates.AnimState.meleeIdle);
        }

        if (weapons[1].activeSelf)
        {
            animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
        }

        if (weapons[2].activeSelf)
        {
            animStates.ChangeAnim(AnimStates.AnimState.revolverIdle);
        }
    }

    private void Reload()
    {
        if (!isReloading)
        {
            if (Input.GetKeyDown(reload))
            {
                if (weapons[1].activeSelf)
                {
                    animStates.ChangeAnim(AnimStates.AnimState.pistolReload);
                }

                if (weapons[2].activeSelf)
                {
                    animStates.ChangeAnim(AnimStates.AnimState.revolverReload);
                }
            }
        }
        else
        {
            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
            }

            if (weapons[2].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.revolverIdle);
            }
        }
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(sprint))
        {
            isSprinting = true;
            if (weapons[0].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.meleeSprint);
            }

            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.pistolSprint);
            }

            if (weapons[2].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.revolverSprint);
            }
        }

        if (Input.GetKeyUp(sprint) && isSprinting)
        {
            isSprinting = false;
            if (weapons[0].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.meleeIdle);
            }

            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
            }

            if (weapons[2].activeSelf)
            {
                animStates.ChangeAnim(AnimStates.AnimState.revolverIdle);
            }
        }
    }
}
