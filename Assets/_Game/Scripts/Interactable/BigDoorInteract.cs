using DG.Tweening;
using UnityEngine;

public class BigDoorInteract : MonoBehaviour
{
    [SerializeField] KeyCode interactKey;
    [SerializeField] Transform door1;
    [SerializeField] Transform door2;
    [SerializeField] float cycleLength = 2f;

    bool isOpenable = false;
    bool open = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOpenable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOpenable = false;
        }
    }

    private void DoorOpen()
    {
        if (Input.GetKeyDown(interactKey))
        {
            open = !open;
            door1.DORotate(new Vector3(0, 90, 0), cycleLength * 0.1f).SetEase(Ease.Linear);
            door2.DORotate(new Vector3(0, -90, 0), cycleLength * 0.1f).SetEase(Ease.Linear);
        }
    }

    private void DoorClose()
    {
        if (Input.GetKeyDown(interactKey))
        {
            open = !open;
            door1.DORotate(new Vector3(0, 0, 0), cycleLength * 0.1f).SetEase(Ease.Linear);
            door2.DORotate(new Vector3(0, 0, 0), cycleLength * 0.1f).SetEase(Ease.Linear);
        }
    }
}
