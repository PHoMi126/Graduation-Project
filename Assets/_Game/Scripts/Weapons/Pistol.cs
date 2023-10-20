using TMPro;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    public TextMeshProUGUI ammoInClip;
    public TextMeshProUGUI ammoReserve;

    private void Start()
    {
        Pistola();
    }

    private void Pistola()
    {
        ammoInClip.text = weaponData.ammoInClip.ToString();
        ammoReserve.text = weaponData.ammoReserve.ToString();
    }
}
