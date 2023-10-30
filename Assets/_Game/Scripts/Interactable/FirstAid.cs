using UnityEngine;

public class FirstAid : MonoBehaviour
{
    [SerializeField] float heal;
    [SerializeField] HPBar playerHP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerHP.currentHP < playerHP.maxHP)
        {
            playerHP.currentHP += heal;

            if (playerHP.currentHP > playerHP.maxHP)
            {
                playerHP.currentHP = playerHP.maxHP;
            }

            Destroy(gameObject);

        }
    }
}
