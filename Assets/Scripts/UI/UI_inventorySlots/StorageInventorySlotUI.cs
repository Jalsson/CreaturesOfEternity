using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using Venus.Utilities;

public class StorageInventorySlotUI : InventorySlotUI
{
    public Storage StorageToHoldItems { private set; get; }

    /// <summary>
    /// Tries to remove item from storage and add it to players inventory
    /// </summary>
    public void OnRemoveButton()
    {
        if (StorageToHoldItems.TryTakeItem(Item))
        {
            if (PlayerManager.S_INSTANCE.player.GetComponent<Storage>().TryStoreItem(Item))
            {
                ClearSlot();
                invenoryUI.UpdateInventoryUI();
            }
        }
    }

    /// <summary>
    /// Adds storage reference that the item can be taken out of the inventory
    /// </summary>
    /// <param name="storage">Storage class of UI component</param>
    public void SetStorageReference(Storage storage)
    {
        StorageToHoldItems = storage;
    }

}
