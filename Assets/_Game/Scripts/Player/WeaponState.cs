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
    [SerializeField] KeyCode pistolSlot;
    [SerializeField] KeyCode revolverSlot;

    [Header("Sound")]
    [SerializeField] AudioClip[] clips;
    [SerializeField] float volume = 1f;

    bool isSprinting;
    bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolIdle);
        AudioSource.PlayClipAtPoint(clips[0], transform.position, volume);
        weapons[0].SetActive(true);
        weapons[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Reload();
        Sprint();
        ChangeWeapon();
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(pistolSlot))
        {
            animStates.ChangeAnimTrigger(AnimStates.AnimState.switchWeapon);
            animStates.animator.SetInteger("weaponType", 2);

            //lambda
            StartCoroutine(IEDelayAction(
                () =>
                {
                    weapons[0].SetActive(true);
                    weapons[1].SetActive(false);
                }, 0.5f));

            animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolIdle);
        }

        if (Input.GetKeyDown(revolverSlot))
        {
            animStates.ChangeAnimTrigger(AnimStates.AnimState.switchWeapon);
            animStates.animator.SetInteger("weaponType", 4);

            StartCoroutine(IEDelayAction(
                () =>
                {
                    weapons[0].SetActive(false);
                    weapons[1].SetActive(true);
                }, 0.5f));

            animStates.ChangeAnimTrigger(AnimStates.AnimState.revolverIdle);

            //StartCoroutine(IEDelayAction(Shoot, 1));

        }
    }

    private IEnumerator IEDelayAction(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (weapons[0].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolShoot);
                AudioSource.PlayClipAtPoint(clips[1], transform.position, volume);
            }

            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.revolverShoot);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (weapons[0].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolIdle);
            }

            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.revolverIdle);
            }
        }
    }

    private void Reload()
    {
        if (!isReloading)
        {
            if (Input.GetKeyDown(reload))
            {
                if (weapons[0].activeSelf)
                {
                    animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolReload);
                }

                if (weapons[1].activeSelf)
                {
                    animStates.ChangeAnimTrigger(AnimStates.AnimState.revolverReload);
                }
            }
        }
        else
        {
            if (weapons[0].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolIdle);
            }

            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.revolverIdle);
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
                animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolSprint);
            }

            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.revolverSprint);
            }
        }

        if (Input.GetKeyUp(sprint) && isSprinting)
        {
            isSprinting = false;
            if (weapons[0].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.pistolIdle);
            }

            if (weapons[1].activeSelf)
            {
                animStates.ChangeAnimTrigger(AnimStates.AnimState.revolverIdle);
            }
        }
    }
}
