using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] AnimStates animStates;
    [SerializeField] StaminaBar sprintFunc;

    [Header("S.Object")]
    [SerializeField] MeleeData meleeData;

    [Header("RayCast Spawn")]
    [SerializeField] Transform cam;

    [Header("Switch Weapon")]
    public Transform ammoCount;

    float timeSinceLastSwing;

    public LayerMask layerMask;
    RaycastHit hit;

    private void Start()
    {
        animStates.ChangeAnim(AnimStates.AnimState.meleeIdle);
        WeaponActions.meleeAttackInput += KnifeAttack;
    }


    private void Update()
    {
        timeSinceLastSwing += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward);
        ammoCount.gameObject.SetActive(false);
    }

    private bool CanSwing() => timeSinceLastSwing > 0.5f / (meleeData.fireRate / 60f);

    public void KnifeAttack()
    {
        if (gameObject.activeSelf)
        {
            if (CanSwing())
            {
                animStates.ChangeAnim(AnimStates.AnimState.meleeAttack);

                if (Physics.Raycast(cam.position, cam.forward, out hit, meleeData.maxDistance, layerMask))
                {
                    IDamagable damagable = hit.transform.GetComponent<IDamagable>();
                    damagable?.TakeDamage(meleeData.damage);
                }
                timeSinceLastSwing = 0f;
            }
            else
            {
                animStates.ChangeAnim(AnimStates.AnimState.meleeIdle);
            }
        }
    }

    private void OnDestroy()
    {
        WeaponActions.meleeAttackInput -= KnifeAttack;
    }
}
