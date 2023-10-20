using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("Stamina UI")]
    [SerializeField] Image staminaBar;

    [Header("Stamina Main")]
    public float currentStam = 100f;
    public float maxStam;
    public float jumpCost = 20f;
    [SerializeField] float sprintCost = 10f;
    internal bool hasRegenerated = true;
    internal bool isSprinting = false;
    internal bool isJumpable = false;


    [Header("Stamina Regen")]
    [SerializeField] float stamRegen = 0.5f;

    [Header("Stamina Affect Speed")]
    public float normalSpeed = 2f;
    public float stamDepleatedSpeed = 1.0f;
    public float sprintSpeed = 3.5f;
    public float crouchSpeed = 1.25f;

    private PlayerAction action;

    private void Start()
    {
        maxStam = currentStam;
        action = GetComponent<PlayerAction>();
    }

    private void Update()
    {
        //Restrict fill amount
        staminaBar.fillAmount = Mathf.Clamp(currentStam / maxStam, 0, 1);

        if (!isSprinting)
        {
            if (currentStam <= maxStam - 0.01)
            {
                currentStam += stamRegen * Time.deltaTime;

                if (currentStam >= 0.2)
                {
                    hasRegenerated = true;
                    action.moveSpeed = normalSpeed;
                }
            }
        }
    }

    public void StamSprint()
    {
        if (hasRegenerated)
        {
            isSprinting = true;
            currentStam -= sprintCost * Time.deltaTime;

            if (currentStam <= 0.1)
            {
                action.moveSpeed = stamDepleatedSpeed;
                hasRegenerated = false;
            }
        }
    }
}
