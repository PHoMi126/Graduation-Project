using UnityEngine;

public class EnemyDealingDMG : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] PlayerAction action;
    [SerializeField] Camera weaponRender;
    [SerializeField] HPBar playerHP;
    private bool damaging = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!damaging)
            {
                //playerHP.currentStam -= heal;
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
