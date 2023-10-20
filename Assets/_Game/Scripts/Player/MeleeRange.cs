using UnityEngine;

public class MeleeRange : MonoBehaviour
{
    [SerializeField] HPBar playerHP;
    [SerializeField] float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            playerHP.currentHP -= damage;
        }
    }
}
