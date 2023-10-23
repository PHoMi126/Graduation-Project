using UnityEngine;

public class FirstAid : MonoBehaviour
{
    [SerializeField] float heal;
    [SerializeField] HPBar playerHP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerHP.currentHP < 100)
        {
            playerHP.currentHP += heal;
        }
    }
}
