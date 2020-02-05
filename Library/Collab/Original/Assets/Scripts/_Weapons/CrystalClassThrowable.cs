using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class CrystalClassThrowable : BaseWeaponClass {

    Camera cam;
    private float speed = 2f;

    private Rigidbody rigidbody;
    private SphereCollider sphereCollider;

    /// <summary>
    /// should be called at creation of this class. Gets references for rigidbody and collider.
    /// Also disables the mentioned classes so that they dont collide with parent 
    /// </summary>
    /// <param name="weaponItem"></param>
    public override void SetWeaponItem(WeaponItem weaponItem)
    {
        rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();

        sphereCollider.enabled = false;
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;
        base.SetWeaponItem(weaponItem);
    }

    /// <summary>
    /// Activates the rigidbody and colliders with small delay and throws the game object at the given position.
    /// </summary>
    /// <param name="targetLocation"></param>
    public void ActivateWeapon(Vector3 targetLocation)
    {
        rigidbody.isKinematic = false;
        StartCoroutine(EnableCollision());
        rigidbody.velocity = (targetLocation - transform.position) * weaponItem.GetWeaponSpeedOrDuration();

        EntityInventory inventory = null;

        if (GetComponentInParent<EntityInventory>())
        {
            inventory = GetComponentInParent<EntityInventory>();
        }

        transform.parent = null;
        inventory.ConsumeEquipment(weaponItem);

    }

    private IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(0.15f);
        sphereCollider.enabled = true;
        rigidbody.detectCollisions = true;
    }

    
    void OnCollisionEnter(Collision col)
    {
        print("detected collision");
        CheckAreaForCollisions();

    }


    /// <summary>
    /// Damages all characters in given range.
    /// </summary>
    private void CheckAreaForCollisions()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, weaponItem.GetStatEffectOrRadius());

        foreach (Collider collider in collidersInRange)
        {
           CharacterStats gotComponent = collider.GetComponent<CharacterStats>();
            
            if (gotComponent != null)
            {
                for (int i = 0; i < 1; i++)
                {
                    gotComponent.TakeDamage(weaponItem.GetWeaponDamage() * weaponDamageModfier);
                }
            }
        }
    }
}
