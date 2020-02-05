using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Delegates;

public class CharacterStats : MonoBehaviour {

    public VoidDelegate Dying
    {
        get
        {
            return characterDying;
        }
        set
        {
            characterDying = value;
        }
    }

    private int startingHealth = 100;
    [SerializeField]
    private int maxHealth;
    public float currentHealth { get; protected set; }

    public bool dead { get; protected set; }
    public bool damaged { get; protected set; }

    [SerializeField]
    protected float nextDamageTickTime = 0.5f;

    // Modifiers that effect damage taken and movement, these have to declared in start to work properly
    public Stat ArmorModifiers { protected set; get ;}
    public Stat MovementModfiers { protected set; get; }


    public VoidDelegate TookDamage;
    protected VoidDelegate characterDying;

    public bool tookDamageRecently { protected set; get; }

    private void Awake()
    {
        currentHealth = maxHealth;
        ArmorModifiers = new Stat();
        MovementModfiers = new Stat();
        characterDying += SpawnLoot;
    }


    /// <summary>
    /// Does damage to the character, takes the armor in to account with armorModifiers.
    /// </summary>
    /// <param name="damageAmount"></param>
    public virtual void TakeDamage (float damageAmount)
    {
        if (!dead)
        {
            FindObjectOfType<AudioManager>().Play("hit");
            FindObjectOfType<AudioManager>().Play("damage");
            tookDamageRecently = true;
            StartCoroutine(ResetDamageTick(nextDamageTickTime));

            damaged = true;
            damageAmount = damageAmount * ArmorModifiers.GetValue();
            damageAmount = Mathf.Clamp(damageAmount, 0, int.MaxValue);

            currentHealth -= damageAmount;
            Debug.Log(transform.name + " takes " + damageAmount + " damage.");


            if (currentHealth <= 0 && !dead)
            {
                dead = true;
                characterDying();
                FindObjectOfType<AudioManager>().Play("Death");

            }
        }
    }

    public virtual void Heal(float healAmount)
    {
        currentHealth += healAmount;
        print("heal");
        print(healAmount);

    }

    /// <summary>
    /// Kills the character and drops the weapons
    /// </summary>
    public virtual void SpawnLoot()
    {
        if (GetComponent<EntityInventory>())
        {
            EntityInventory oldStorage = GetComponent<EntityInventory>();
            Storage newStorage = gameObject.AddComponent<Storage>();

            for (int i = 0; i < oldStorage.Items.Count; i++)
            {
                newStorage.TryStoreItem(oldStorage.Items[i]);
            }

            Destroy(oldStorage);
            StorageInteractible interactible = gameObject.AddComponent<StorageInteractible>();
            interactible.LocalStorage = newStorage;
            Destroy(GetComponent<CapsuleCollider>());

            if (GetComponent<ArmorAndWeaponEquipper>())
            {
                GameObject[] equipedWeapons = GetComponent<ArmorAndWeaponEquipper>().EquipedWeapons;

                for (int i = 0; i < equipedWeapons.Length; i++)
                {
                    if (equipedWeapons[i])
                    {
                        ItemPickup itemPickup = equipedWeapons[i].AddComponent<ItemPickup>();
                        itemPickup.AddItem(equipedWeapons[i].GetComponent<WeaponClass>().WeaponItem);
                        

                        equipedWeapons[i].AddComponent<BoxCollider>();

                        equipedWeapons[i].transform.parent = null;

                        Rigidbody rb = equipedWeapons[i].GetComponent<Rigidbody>();
                        rb.mass = 0.1f;
                        rb.drag = 4;
                        rb.AddForce(new Vector3(Random.Range(0,10), 60,Random.Range(0,10)));
                    }
                }
            }
        }



    }

    /// <summary>
    /// Prevents character for taking damage for certain amount of time once it has been damaged.
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    protected IEnumerator ResetDamageTick(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        tookDamageRecently = false;
    }

    /// <summary>
    /// This is called when wanting to apply temporary effect. 
    /// We are using this middle method here in case item that is calling coroutine is destroyed. 
    /// </summary>
    /// <param name="weaponItem"></param>
    public void StartTempStatEffect(WeaponItem weaponItem)
    {
        StartCoroutine(AddTemporaryStatEffect(weaponItem));
    }

    public IEnumerator AddTemporaryStatEffect(WeaponItem weaponItem)
    {
        switch (weaponItem.CrystalTypeEnum)
        {
            case CrystalTypeEnum.None:
                break;
            case CrystalTypeEnum.Explosive:
                break;
            case CrystalTypeEnum.ArmorBuff:
                ArmorModifiers.AddModifier(weaponItem.StatEffectOrRadius);
                break;
            case CrystalTypeEnum.SpeedBuff:
                MovementModfiers.AddModifier(weaponItem.StatEffectOrRadius);
                break;
            case CrystalTypeEnum.NumberOfTypes:
                break;
            case CrystalTypeEnum.HealthBuff:
                Heal(weaponItem.StatEffectOrRadius);
                break;
            default:
                break;
        }
        if (this is PlayerStats)
        {
            PlayerStats playerStats = (PlayerStats)this;
            playerStats.UpdateStatus();
        }

        yield return new WaitForSeconds(weaponItem.AttackSpeedOrDuration);
        switch (weaponItem.CrystalTypeEnum)
        {
            case CrystalTypeEnum.None:
                break;
            case CrystalTypeEnum.Explosive:
                break;
            case CrystalTypeEnum.ArmorBuff:
                ArmorModifiers.RemoveModifier(weaponItem.StatEffectOrRadius);
                break;
            case CrystalTypeEnum.SpeedBuff:
                MovementModfiers.RemoveModifier(weaponItem.StatEffectOrRadius);
                break;
            case CrystalTypeEnum.NumberOfTypes:
                break;
            default:
                break;
        }
        if (this is PlayerStats)
        {
            PlayerStats playerStats = (PlayerStats)this;
            playerStats.UpdateStatus();
        }
    }
}
