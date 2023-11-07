using UnityEngine;

public class AnimStates : MonoBehaviour
{
    [SerializeField] internal Animator animator;

    private AnimState currentAnimState;
    internal bool isDead;

    public enum AnimState
    {
        //Player
        switchWeapon,
        meleeIdle, meleeAttack, meleeSprint,
        pistolIdle, pistolShoot, pistolSprint, pistolReload,
        rifleIdle, rifleShoot, rifleSprint, rifleReload,
        revolverIdle, revolverShoot, revolverSprint, revolverReload,
        dead,

        //Enemy
        zIdle, zAttack, zWalk, zRun, zDead
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool isTesr = false;

    public void ChangeAnim(AnimState _state)
    {
        if (isDead == true)
            return;
        if (currentAnimState != _state)
        {
            //animator.ResetTrigger(currentAnimState.ToString());
            currentAnimState = _state;
            animator.SetTrigger(currentAnimState.ToString());

            if (isTesr == true)
            {
                Debug.Log(_state);
            }
        }


    }

    public void ResetAnim()
    {
        if (isTesr == true)
        {
            Debug.LogError(currentAnimState);
        }
        animator.ResetTrigger(currentAnimState.ToString());
    }
}
