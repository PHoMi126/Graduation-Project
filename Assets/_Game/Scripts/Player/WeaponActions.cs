using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActions : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] KeyCode reload;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }

        if (Input.GetKeyDown(reload))
        {
            reloadInput?.Invoke();
        }
    }
}
