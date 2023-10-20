using UnityEngine;

public class EnemyDealingDMG : MonoBehaviour
{
    //[SerializeField] HPBar playerHP;
    [SerializeField] float damage;
    [SerializeField] PlayerAction action;
    [SerializeField] Camera weaponRender;
    private bool damaging = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!damaging)
            {
                //playerHP.currentStam -= heal;
                other.GetComponent<HPBar>().currentHP -= damage;
            }

            if (other.GetComponent<HPBar>().currentHP <= 0)
            {
                action.enabled = false;
                weaponRender.gameObject.SetActive(false);
            }
        }
    }
}
