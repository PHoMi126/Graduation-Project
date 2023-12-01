using DG.Tweening;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] KeyCode interactKey;
    [SerializeField] Transform elevator;
    [SerializeField] float cycleLength = 2f;

    bool isUseable = false;
    bool use = false;

    private void Update()
    {
        if (isUseable)
        {
            ElevatorMove();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isUseable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isUseable = false;
        }
    }

    private void ElevatorMove()
    {
        if (Input.GetKeyDown(interactKey))
        {
            use = true;
            elevator.DOLocalMoveY(6.3065f, cycleLength * 3f).SetEase(Ease.Linear);
            use = false;
        }
    }
}
