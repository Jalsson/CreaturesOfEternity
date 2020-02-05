using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{

    WeaponClass weaponClass;

    private void Start()
    {
        weaponClass = GetComponentInParent<WeaponClass>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (weaponClass)
            weaponClass.OnTriggerEnter(col);
        else
            weaponClass = GetComponentInParent<WeaponClass>();
    }
}