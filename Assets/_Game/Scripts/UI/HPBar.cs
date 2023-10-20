using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Image healthBar;
    public float currentHP = 100f;
    public float maxHP;

    private void Awake()
    {
        maxHP = currentHP;
    }

    private void Update()
    {
        //Restrict fill amount
        healthBar.fillAmount = Mathf.Clamp(currentHP / maxHP, 0, 1);
    }
}
