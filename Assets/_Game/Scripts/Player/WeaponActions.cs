using System;
using UnityEngine;

public class WeaponActions : MonoBehaviour
{
    public static Action AttackInput;
    public static Action reloadInput;

    [SerializeField] KeyCode reload;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttackInput?.Invoke();
        }

        if (Input.GetKeyDown(reload))
        {
            reloadInput?.Invoke();
        }
    }
}
