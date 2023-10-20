using UnityEngine;

public class FirstAid : MonoBehaviour
{
    [SerializeField] float heal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HPBar>().currentHP += heal;
        }
    }
}
