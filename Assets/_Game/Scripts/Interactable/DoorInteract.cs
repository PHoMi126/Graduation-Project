using DG.Tweening;
using UnityEngine;

public class DoorInteract : DoorsFather
{
    private void Update()
    {
        if (isOpenable)
        {
            if (open == true)
            {
                DoorOpen();
            }
            else
            {
                DoorClose();
            }
        }
    }

    private void DoorOpen()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (door2 == null)
            {
                open = !open;
                door1.DOLocalRotate(new Vector3(0, -90, 0), cycleLength * 0.5f).SetEase(Ease.Linear);
            }
        }
    }

    private void DoorClose()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (door2 == null)
            {
                open = !open;
                door1.DOLocalRotate(new Vector3(0, 0, 0), cycleLength * 0.5f).SetEase(Ease.Linear);
            }
        }
    }
}
