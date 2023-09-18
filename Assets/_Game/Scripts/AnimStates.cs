using UnityEngine;

public class AnimStates : MonoBehaviour
{
    [SerializeField] Animator animator;

    private AnimState currentAnimState;
    bool isDead;

    public enum AnimState
    {
        idle,
        weaponIdle, weaponShoot, weaponSprint, weaponReload
    }

    public void ChangeAnim(AnimState _state)
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
