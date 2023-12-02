using System;
using UnityEngine;

public class WeaponActions : MonoBehaviour
{
    public static Action attackInput;
    public static Action meleeAttackInput;
    public static Action reloadInput;

    [SerializeField] KeyCode reload;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attackInput?.Invoke();
        }

        if (Input.GetKeyDown(reload))
        {
            reloadInput?.Invoke();
        }

        if (Input.GetMouseButton(0))
        {
            meleeAttackInput?.Invoke();
        }
    }
}
