using UnityEngine;

public class AnimStates : MonoBehaviour
{
    [SerializeField] Animator animator;

    private AnimState currentAnimState;
    bool isDead;

    public enum AnimState
    {
        idle, walk, sprint, jump, pistolPose
    }

    public void ChangeAnim(AnimState _state)
    {
        if (isDead == true)
            return;
        if (currentAnimState != _state)
        {
            currentAnimState = _state;
            animator.SetTrigger(currentAnimState.ToString());
        }
    }
}
