using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [Header("Weapon Switch")]
    [SerializeField] Transform[] weapons;
    [SerializeField] KeyCode[] weaponSwitchKeys;
    [SerializeField] float switchTimer;

    [Header("Switch Animation")]
    [SerializeField] AnimStates animStates;

    [Header("Sounds")]
    public AudioSource weaponSwitchSound;

    int selectedWeapon;
    float timeSinceLastSwitch;

    private void Start()
    {
        weapons[0].gameObject.SetActive(true);
        SetWeapons();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    private void Update()
    {
        //Local variable hold index of last selected weapon
        int preSelectedWeapon = selectedWeapon;

        for (int i = 0; i < weaponSwitchKeys.Length; i++)
        {
            if (Input.GetKeyDown(weaponSwitchKeys[i]) && timeSinceLastSwitch >= switchTimer)
            {
                selectedWeapon = i;
            }

            //Change weapon if selected weapon different from previous weapon
            if (preSelectedWeapon != selectedWeapon) Select(selectedWeapon);

            timeSinceLastSwitch += Time.deltaTime;
        }
    }

    private void SetWeapons()
    {
        //Number of weapons currently in inventory
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        //Set new KeyCode in array if null
        weaponSwitchKeys ??= new KeyCode[weapons.Length];
    }

    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            //Set weapon active/inactive
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        timeSinceLastSwitch = 0f;

        OnWeaponSelected();
    }

    private void OnWeaponSelected()
    {
        animStates.ChangeAnim(AnimStates.AnimState.switchWeapon);
    }
}
