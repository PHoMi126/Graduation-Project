using UnityEngine;

public class AnimStates : MonoBehaviour
{
    [SerializeField] internal Animator animator;

    private AnimState currentAnimState;
    bool isDead;

    public enum AnimState
    {
        idle,
        switchWeapon,
        pistolIdle, pistolShoot, pistolSprint, pistolReload,
        rifleIdle, rifleShoot, rifleSprint, rifleReload,
        revolverIdle, revolverShoot, revolverSprint, revolverReload
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimTrigger(AnimState _state)
    {
        if (isDead == true)
            return;
        if (currentAnimState != _state)
        {
            //animator.ResetTrigger(currentAnimState.ToString());
            currentAnimState = _state;
            animator.SetTrigger(currentAnimState.ToString());
        }
    }
}
