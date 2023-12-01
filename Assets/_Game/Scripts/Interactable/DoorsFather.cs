using UnityEngine;

public class DoorsFather : MonoBehaviour
{
    [SerializeField] protected KeyCode interactKey;
    [SerializeField] protected Transform door1;
    [SerializeField] protected Transform door2;
    [SerializeField] protected float cycleLength = 2f;

    protected bool isOpenable = false;
    protected bool open = false;

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
}
