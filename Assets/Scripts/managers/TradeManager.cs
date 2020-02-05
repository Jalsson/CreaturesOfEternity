using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Venus.Utilities;

/// <summary>
/// Handles the trading between player and a given entity when initated.
/// </summary>
public class TradeManager : Singleton<TradeManager>
{
    #region Variables

    /// <summary>
    /// The entity inventory of the player
    /// </summary>
    [SerializeField] //Seriealize for testing
    private EntityInventory playerInventory;

    /// <summary>
    /// The enity inventory of the entity in trade with
    /// </summary>
    private EntityInventory entityInventory;
    
    /// <summary>
    /// Storage holding all the items that the player has put into the trade.
    /// </summary>
    private Storage playerTradeStorage;

    /// <summary>
    /// Storage holding all the items that the entity has put into the trade.
    /// </summary>
    private Storage entityTradeStorage;

    /// <summary>
    /// Slots shown on the left displaying player's inventory's items.
    /// </summary>
    private TradeInventorySlot[] playerInventorySlots;

    /// <summary>
    /// Slots shown on the right displaying entity's inventory's items.
    /// </summary>
    private TradeInventorySlot[] entityInventorySlots;

    /// <summary>
    /// Slots on top of the middleman displaying player's items currently being traded
    /// </summary>
    private TradeInventorySlot[] playerTradeSlots;
    /// <summary>
    /// Slots on bottom of the middleman displaying entity's items currently being traded
    /// </summary>
    private TradeInventorySlot[] entityTradeSlots;

    /// <summary>
    /// Money in trade. Negative means that the player will pay.
    /// </summary>
    private int moneyToPlayer = 0;

    /// <summary>
    /// Free slots in the trade storages.
    /// </summary>
    private int tradeStorageSpace = 12;

    #endregion

    #region UI variables

    /// <summary>
    /// Panel containing all the UI elements shown on the trade screen.
    /// </summary>
    private RectTransform tradePanel;

    /// <summary>
    /// Button to confirm the trade with.
    /// </summary>
    private Button confirmButton;

    /// <summary>
    /// Text displaying the current trade's profit or loss to the player
    /// </summary>
    private TextMeshProUGUI profitLossText;

    /// <summary>
    /// Text displaying the player's money amount
    /// </summary>
    private TextMeshProUGUI playerMoneyText;

    /// <summary>
    /// Text displaying the entity's money amount
    /// </summary>
    private TextMeshProUGUI entityMoneyText;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        if (PlayerManager.S_INSTANCE != null)
        {
            playerInventory = PlayerManager.S_INSTANCE.player.GetComponent<EntityInventory>();
        }

        //Create instances for trading
        playerTradeStorage = new Storage();
        entityTradeStorage = new Storage();

        //Set max item amounts
        playerTradeStorage.SetMaxItemAmount(tradeStorageSpace);
        entityTradeStorage.SetMaxItemAmount(tradeStorageSpace);

        //Trading panel
        tradePanel = transform.Find("TradingPanel").GetComponent<RectTransform>();

        //Inventory slot references
        playerInventorySlots = tradePanel.Find("PlayerInventory").GetComponentsInChildren<TradeInventorySlot>();
        entityInventorySlots = tradePanel.Find("EntityInventory").GetComponentsInChildren<TradeInventorySlot>();

        //Money texts
        playerMoneyText = tradePanel.Find("PlayerInventory").Find("MoneyText").GetComponent<TextMeshProUGUI>();
        entityMoneyText = tradePanel.Find("EntityInventory").Find("MoneyText").GetComponent<TextMeshProUGUI>();

        //Trade slot references
        playerTradeSlots = tradePanel.Find("MiddleMan").Find("PlayerTradeItems").GetComponentsInChildren<TradeInventorySlot>();
        entityTradeSlots = tradePanel.Find("MiddleMan").Find("EntityTradeItems").GetComponentsInChildren<TradeInventorySlot>();

        //Profit/Loss text
        profitLossText = tradePanel.Find("MiddleMan").Find("ProfitLossText").GetComponent<TextMeshProUGUI>();

        //Confirm button reference
        confirmButton = tradePanel.Find("ConfirmButton").GetComponent<Button>();
    }

    /// <summary>
    /// Initiates the trade between the player and the given entity.
    /// </summary>
    /// <param name="entityInventory">Entity to trade with.</param>
    public void StartTrade (EntityInventory entityInventory)
    {
        if (playerInventory == null || entityInventory == null)
        {
            Debug.LogWarning("WARNING: Tried to start a trade but one of the inventories in the trade were null!");
            return;
        }

        InputManager.S_INSTANCE.UiState = UiStateEnum.Trading;

        //Assign InputManager delegates
        InputManager.S_INSTANCE.Escape += CancelTrade;
        InputManager.S_INSTANCE.CanInteract = false;
        InputManager.S_INSTANCE.InputVector += CheckForMovementCancel;

        this.entityInventory = entityInventory;

        tradePanel.gameObject.SetActive(true);

        UpdateUI();
    }

    /// <summary>
    /// Cancels the trade in process and returns the items to their owners.
    /// </summary>
    public void CancelTrade ()
    {
        InputManager.S_INSTANCE.UiState = UiStateEnum.GamePlay;

        //Unassign InputManager delegates
        InputManager.S_INSTANCE.Escape -= CancelTrade;
        InputManager.S_INSTANCE.CanInteract = true;
        InputManager.S_INSTANCE.InputVector -= CheckForMovementCancel;

        //Take items back.
        TradeAlItems(playerTradeStorage, playerInventory);
        TradeAlItems(entityTradeStorage, entityInventory);

        moneyToPlayer = 0;

        tradePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Checks if the trade is possible and returns a bool indicating it.
    /// </summary>
    /// <returns></returns>
    public bool TradePossible ()
    {
        if (entityTradeStorage.Items.Count < 1 && playerTradeStorage.Items.Count < 1) //No items in trade
        {
            return false;
        }

        if (playerInventory.FreeSlotsLeft() < entityTradeStorage.Items.Count) //Can't fit items in player's invetory
        {
            return false;
        }
        
        if (moneyToPlayer < 0) //Money goes to entity
        {
            return playerInventory.CanAfford(Mathf.Abs(moneyToPlayer));
        }
        else if (moneyToPlayer > 0) //Money goes to player
        {
            return entityInventory.CanAfford(Mathf.Abs(moneyToPlayer));
        }

        return true;
    }

    /// <summary>
    /// Confirms the trade in progress.
    /// </summary>
    public void ConfirmTrade ()
    {
        if (TradePossible())
        {
            //Trade items from player's trade storage to entity's inventory and vice versa
            TradeAlItems(playerTradeStorage, entityInventory);
            TradeAlItems(entityTradeStorage, playerInventory);

            if (moneyToPlayer < 0) //Money goes to entity
            {
                playerInventory.TradeMoney(Mathf.Abs(moneyToPlayer), entityInventory);
            }
            else if (moneyToPlayer > 0) //Money goes to player
            {
                entityInventory.TradeMoney(Mathf.Abs(moneyToPlayer), playerInventory);
            }

            moneyToPlayer = 0;

            UpdateUI();
        }
    }

    /// <summary>
    /// Trades all the items from the sender's storage to the receiver's storage.
    /// </summary>
    /// <param name="sender">Storage to trade the items from.</param>
    /// <param name="receiver">Storage to trade the items to.</param>
    private void TradeAlItems (Storage sender, Storage receiver)
    {
        if (sender.Items.Count > 0) //Make sure there's items to trade
        {
            Item[] itemsToTrade = sender.Items.ToArray();

            foreach (Item item in itemsToTrade) //Loop all the items
            {
                if (!TryTradeItem(item, sender, receiver)) //Try to trade
                {
                    Debug.LogWarning("WARNING: item could not be traded for some reason. Either the item was not in the sender's inventory or the receiver didn't have enough space for it.");
                }
            }
        }
    }

    /// <summary>
    /// Tries to trade the given item from the sender to the receiver.
    /// </summary>
    /// <param name="item">Item to trade.</param>
    /// <param name="sender">Storage that the item is currently in.</param>
    /// <param name="receiver">Strorage that the item will be stored into.</param>
    /// <returns>True if successful.</returns>
    private bool TryTradeItem (Item item, Storage sender, Storage receiver)
    {
        if (!sender.TryTakeItem(item)) //Take item
        {
            return false;
        }

        return receiver.TryStoreItem(item); //Store item
    }

    #region UI slot clicking functions

    /// <summary>
    /// Tries to add the item from the inventory slot to the player trade storage.
    /// </summary>
    /// <param name="slotClicked">Slot the command came from and the item will get traded from.</param>
    public void TryAddItemToPlayerTrade (TradeInventorySlot slotClicked)
    {
        if (slotClicked.Item == null)
        {
            return;
        }

        if (playerTradeStorage.FreeSlotsLeft() > 0)
        {
            TryTradeItem(slotClicked.Item, playerInventory, playerTradeStorage);

            moneyToPlayer += slotClicked.Item.Price;
        }

        UpdateUI();
    }

    /// <summary>
    /// Tries to add the item from the inventory slot to the entity trade storage.
    /// </summary>
    /// <param name="slotClicked">Slot the command came from and the item will get traded from.</param>
    public void TryAddItemToEntityTrade(TradeInventorySlot slotClicked)
    {
        if (slotClicked.Item == null)
        {
            return;
        }

        if (entityTradeStorage.FreeSlotsLeft() > 0)
        {
            TryTradeItem(slotClicked.Item, entityInventory, entityTradeStorage);

            moneyToPlayer -= slotClicked.Item.Price;
        }

        UpdateUI();
    }

    /// <summary>
    /// Adds the given item back to the entity's inventory from the trade storage.
    /// </summary>
    /// <param name="slotClicked">Slot the command came from and the item will get traded from.</param>
    public void AddItemBackToEntityInventory (TradeInventorySlot slotClicked)
    {
        if (slotClicked.Item == null)
        {
            return;
        }

        TryTradeItem(slotClicked.Item, entityTradeStorage, entityInventory);

        moneyToPlayer += slotClicked.Item.Price;

        UpdateUI();
    }

    /// <summary>
    /// Adds the given item back to player's inventory from the trade storage.
    /// </summary>
    /// <param name="slotClicked">Slot the command came from and the item will get traded from.</param>
    public void AddItemBackToPlayerInventory(TradeInventorySlot slotClicked)
    {
        if (slotClicked.Item == null)
        {
            return;
        }

        TryTradeItem(slotClicked.Item, playerTradeStorage, playerInventory);

        moneyToPlayer -= slotClicked.Item.Price;

        UpdateUI();
    }

    #endregion

    /// <summary>
    /// Refreshes all the UI slots
    /// </summary>
    protected void UpdateUI ()
    {
        //Update items in inventories
        UpdateSlotGroup(playerInventorySlots, playerInventory);
        UpdateSlotGroup(entityInventorySlots, entityInventory);

        //Update items in trade
        UpdateSlotGroup(playerTradeSlots, playerTradeStorage);
        UpdateSlotGroup(entityTradeSlots, entityTradeStorage);

        //Disable confirm is trade is not possible
        confirmButton.interactable = TradePossible();

        //Money text
        playerMoneyText.text = string.Format("Money: {0}", playerInventory.Money);
        entityMoneyText.text = string.Format("Money: {0}", entityInventory.Money);

        //Profit/Loss text
        profitLossText.text = moneyToPlayer.ToString();
    }

    /// <summary>
    /// Updates the given TradeInventorySlots to show the given storage's items.
    /// </summary>
    /// <param name="slotGroup">UI slots to display the items in.</param>
    /// <param name="storage">Storage to display the items from.</param>
    protected void UpdateSlotGroup(TradeInventorySlot[] slotGroup, Storage storage)
    {
        for (int i = 0; i < slotGroup.Length; i++)
        {
            if (storage.Items.Count <= i) //Clear
            {
                slotGroup[i].ClearSlot();
            }
            else //Set
            {
                slotGroup[i].AddItem(storage.Items[i]);
            }
        }
    }

    private void CheckForMovementCancel (Vector3 vector)
    {
        if (vector != Vector3.zero)
        {
            CancelTrade();
        }
    }
}
