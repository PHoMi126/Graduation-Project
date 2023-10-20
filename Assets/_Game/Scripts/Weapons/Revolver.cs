using TMPro;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    public TextMeshProUGUI ammoInClip;
    public TextMeshProUGUI ammoReserve;

    private void Start()
    {
        Revolva();
    }

    private void Revolva()
    {
        ammoInClip.text = weaponData.ammoInClip.ToString();
        ammoReserve.text = weaponData.ammoReserve.ToString();
    }
}
