using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;
using Venus.Utilities;

public class ItemAiManager : Singleton<ItemAiManager>
{

    public List<AiMovementController> aiControllerList = new List<AiMovementController>();


    public Object[] allItems { private set; get; }
    private List<WeaponItem> weapons = new List<WeaponItem>();
    private List<ArmorItem> armors = new List<ArmorItem>();
    private List<Item> items = new List<Item>();
    

    public float circleDirection = 0f;


    public void Update()
    {
        for (int i = 0; i < aiControllerList.Count; i++)
        {
            if (aiControllerList[i].positionInRange % 2 == 0)
            {
                circleDirection = 0f;
            }

            else
            {
                circleDirection = 1f;
            }
        }
    }
    
    public void UpdateAIBehaviors()
    {
        aiControllerList.Clear();
    }


    protected override void Awake()
    {
        base.Awake();
        allItems = Resources.LoadAll("Items", typeof(Item));

        //Sorting all items to sub lists of each type
        foreach (var item in allItems)
        {
            if (item is WeaponItem)
            {
                weapons.Add((WeaponItem)item);
            }
            else if (item is ArmorItem)
            {
                armors.Add((ArmorItem)item);
            }
            else if (item is Item)
            {
                items.Add((Item)item);
            }
        }

      

    }

    /// <summary>
    /// Equips given inventory with random equipment for specific rarity level.
    /// </summary>
    /// <param name="entityInventory">iventory that wants to effected</param>
    /// <param name="rarityEnum">Rarity level of wanted Items</param>
    /// <param name="randomItems">How many random Items is wanted insideIventory</param>
    public void EquipEquipment(EntityInventory entityInventory, RarityEnum rarityEnum, int randomItems)
    {
        List<Item> itemsToEquip = GetEquipment(rarityEnum, weapons, armors, items, randomItems);

        for (int i = 0; i < itemsToEquip.Count; i++)
        {
            if (itemsToEquip[i] is WeaponItem || itemsToEquip[i] is ArmorItem)
            {
                entityInventory.TryEquipItem(itemsToEquip[i]);
            }
            else
            {
                entityInventory.TryStoreItem(itemsToEquip[i]);
            }
        }
    }

    /// <summary>
    /// Shorts all the items by rarity and returns a list with primary weapon and armor set, and given amount of items
    /// </summary>
    /// <param name="rarityEnum">Rarity of the item</param>
    /// <param name="weapons">weapons to loop through</param>
    /// <param name="armors">armors to loop through</param>
    /// <param name="items">items to loop through</param>
    /// <param name="numberOfItems">how many random items included</param>
    /// <returns></returns>
    public List<Item> GetEquipment(RarityEnum rarityEnum, List<WeaponItem> weapons, List<ArmorItem> armors, List<Item> items, int numberOfItems)
    {
        List<Item> itemsToReturn = new List<Item>();

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].WeaponSlotEnum == WeaponSlotEnum.Secondary)
            {
                weapons.Remove(weapons[i]);
            }
            if (weapons[i].RarityEnum != rarityEnum)
            {
                weapons.Remove(weapons[i]);
            }
        }
        itemsToReturn.Add(weapons[Random.Range(0, weapons.Count)]);

        List<ArmorItem>[] arrayListOfSlots = new List<ArmorItem>[4];
        for (int i = 0; i < arrayListOfSlots.Length; i++)
        {
            arrayListOfSlots[i] = new List<ArmorItem>();
        }

        for (int i = 0; i < armors.Count; i++)
        {
            if (armors[i].RarityEnum != rarityEnum)
            {
                armors.Remove(armors[i]);
            }
            else
            {
                switch (armors[i].ArmorEquipSlotEnum)
                {
                    case ArmorSlotEnum.Head:
                        arrayListOfSlots[0].Add(armors[i]);
                        break;
                    case ArmorSlotEnum.Chest:

                        arrayListOfSlots[1].Add(armors[i]);
                        break;
                    case ArmorSlotEnum.Belt:
                        arrayListOfSlots[2].Add(armors[i]);
                        break;

                    case ArmorSlotEnum.Legs:
                        arrayListOfSlots[3].Add(armors[i]);
                        break;
                    default:
                        break;
                }
            }

        }

        for (int i = 0; i < (int)ArmorSlotEnum.NumberOfTypes; i++)
        {
            itemsToReturn.Add(arrayListOfSlots[i][Random.Range(0, arrayListOfSlots[i].Count)]);
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].RarityEnum != rarityEnum)
            {
                items.Remove(items[i]);
            }
        }

        for (int i = 0; i < numberOfItems; i++)
        {
            itemsToReturn.Add(items[Random.Range(0, items.Count)]);
        }

        return itemsToReturn;
    }

}
