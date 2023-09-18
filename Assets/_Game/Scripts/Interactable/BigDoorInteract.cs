using DG.Tweening;
using UnityEngine;

public class BigDoorInteract : MonoBehaviour
{
    [SerializeField] KeyCode interactKey;
    [SerializeField] Transform door1;
    [SerializeField] Transform door2;
    [SerializeField] float cycleLength = 2f;

    bool isOpenable = true;

    private void Update()
    {
        if (door1 != null && door2 != null)
        {
            DoorAction();
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

    private void DoorAction()
    {
        if (isOpenable && Input.GetKeyDown(interactKey))
        {
            door1.DORotate(new Vector3(0, 90, 0), cycleLength * 0.5f)
            .SetLoops(loops: (int)LoopType.Yoyo)
            .SetEase(Ease.Linear);

            door2.DORotate(new Vector3(0, -90, 0), cycleLength * 0.5f)
            .SetLoops(loops: (int)LoopType.Yoyo)
            .SetEase(Ease.Linear);
        }
    }
}
