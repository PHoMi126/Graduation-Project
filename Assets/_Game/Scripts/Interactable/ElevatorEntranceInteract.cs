using DG.Tweening;
using UnityEngine;

public class ElevatorEntranceInteract : DoorsFather
{
    private void Update()
    {
        if (isOpenable)
        {
            DoorOpen();
        }
    }

    private void DoorOpen()
    {
        if (Input.GetKeyDown(interactKey))
        {
            open = true;
            door1.DOLocalMoveZ(0.848f, cycleLength).SetEase(Ease.Linear);
            door2.DOLocalMoveZ(2.162f, cycleLength).SetEase(Ease.Linear);
        }
    }
}
