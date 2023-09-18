using UnityEngine;

public class WeaponState : MonoBehaviour
{
    [SerializeField] AnimStates animStates;
    [SerializeField] KeyCode reload;
    [SerializeField] KeyCode sprint;

    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        animStates.ChangeAnim(AnimStates.AnimState.weaponIdle);
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Reload();
        Sprint();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animStates.ChangeAnim(AnimStates.AnimState.weaponShoot);
        }

        if (Input.GetMouseButtonUp(0))
        {
            animStates.ChangeAnim(AnimStates.AnimState.weaponIdle);
        }
    }

    private void Reload()
    {
        if (Input.GetKey(reload))
        {
            animStates.ChangeAnim(AnimStates.AnimState.weaponReload);
        }
        else
        {
            animStates.ChangeAnim(AnimStates.AnimState.weaponIdle);
        }
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(sprint))
        {
            Debug.Log(sprint + "Down");
            isSprinting = true;
            animStates.ChangeAnim(AnimStates.AnimState.weaponSprint);
        }

        if (Input.GetKeyUp(sprint) && isSprinting)
        {
            Debug.Log(sprint + "Up");
            isSprinting = false;
            animStates.ChangeAnim(AnimStates.AnimState.weaponIdle);
        }
    }
}
