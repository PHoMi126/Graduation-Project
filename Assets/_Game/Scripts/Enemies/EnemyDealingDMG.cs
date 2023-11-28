using UnityEngine;

public class EnemyDealingDMG : MonoBehaviour
{
    [SerializeField] PlayerAction action;
    [SerializeField] Camera weaponRender;
    [SerializeField] HPBar playerHP;
    private bool damaging = false;
    private float damage;

    private void Update()
    {
        damage = Random.Range(2, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!damaging)
            {
                playerHP.currentHP -= damage;
            }

            if (playerHP.currentHP <= 0)
            {
                action.enabled = false;
                weaponRender.gameObject.SetActive(false);
            }
        }
    }
}
