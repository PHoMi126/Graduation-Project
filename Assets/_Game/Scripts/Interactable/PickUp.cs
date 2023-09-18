using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] AnimStates animStates;
    [SerializeField] WeaponState weaponAnim;
    [SerializeField] Transform pistol;

    bool isPickable = true;
    // Start is called before the first frame update
    void Start()
    {
        animStates.ChangeAnim(AnimStates.AnimState.idle);
        weaponAnim.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WeaponPickUp"))
        {
            Destroy(pistol.gameObject);
            weaponAnim.enabled = true;
        }
    }
}
