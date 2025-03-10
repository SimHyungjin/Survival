using System;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public GameObject infoWindow;
    public Transform slotPanel;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject dropBtn;

    private PlayerController controller;

    public Action addItem;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;
        inventoryWindow = gameObject;
        inventoryWindow.SetActive(false);
        infoWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].uIInventory = this;
            slots[i].quantity = -1;
        }
        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useBtn.SetActive(false);
        equipBtn.SetActive(false);
        unEquipBtn.SetActive(false);
        dropBtn.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            UpdateUI();
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                addItem?.Invoke();
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.ItemData = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            addItem?.Invoke();
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
        return;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemData != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemData == data)
            {
                return slots[i];
            }
        }
        return null;
    }
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemData == null)
                return slots[i];
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].equipped)
                UnEquip(i);
        }
        Instantiate(data.dropPrefab, CharacterManager.Instance.Player.dropPos.position, Quaternion.identity);
        RemoveSelectedItem();
        CharacterManager.Instance.Player.dropItem?.Invoke();
    }

    public void SelectItem(int index)
    {
        if (slots[index].ItemData == null)
        {
            infoWindow.SetActive(false);
            return;
        }
        infoWindow.SetActive(true);
        selectedItem = slots[index].ItemData;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.ItemName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.usable.Length; i++)
        {
            selectedStatName.text += selectedItem.usable[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.usable[i].value.ToString() + "\n";
        }

        useBtn.SetActive(selectedItem.type == ItemType.Usable);
        equipBtn.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unEquipBtn.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropBtn.SetActive(true);
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].ItemData = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public void RemoveItemNum(ItemData data, int num)
    {
        foreach (var slot in slots)
        {
            if (slot.ItemData.ItemName == data.ItemName)
            {
                slot.quantity -= num;
                if (slot.quantity <= 0)
                {
                    slot.ItemData = null;
                    slot.index = -1;
                }
                break;
            }
        }
        UpdateUI();
    }

    public int GetItemCount(ItemData item)
    {
        int totalCount = 0;
        foreach (var slot in slots)
        {
            if (slot.ItemData == item)
            {
                totalCount += slot.quantity;
                break;
            }
        }
        return totalCount;
    }

    public void OnUseBtn()
    {
        if (selectedItem.type == ItemType.Usable)
        {
            for (int i = 0; i < selectedItem.usable.Length; i++)
            {
                if (selectedItem.usable[i].type == UsableType.Health && selectedItem.usable[i].usingType == UsingType.Instant)
                {
                    CharacterManager.Instance.Player.condition.health.Add(selectedItem.usable[i].value);
                }
                else if (selectedItem.usable[i].type == UsableType.Hunger && selectedItem.usable[i].usingType == UsingType.Instant)
                {
                    CharacterManager.Instance.Player.condition.hunger.Add(selectedItem.usable[i].value);
                }
                else if (selectedItem.usable[i].type == UsableType.Stamina && selectedItem.usable[i].usingType == UsingType.Instant)
                {
                    CharacterManager.Instance.Player.condition.stamina.Add(selectedItem.usable[i].value);
                }
                else if (selectedItem.usable[i].type == UsableType.Health && selectedItem.usable[i].usingType == UsingType.Gradual)
                {
                    StartCoroutine(CharacterManager.Instance.Player.condition.health.CoroutineAdd(selectedItem.usable[i].value, selectedItem.usable[i].time));
                }
                else if (selectedItem.usable[i].type == UsableType.Hunger && selectedItem.usable[i].usingType == UsingType.Gradual)
                {
                    StartCoroutine(CharacterManager.Instance.Player.condition.hunger.CoroutineAdd(selectedItem.usable[i].value, selectedItem.usable[i].time));
                }
                else if (selectedItem.usable[i].type == UsableType.Stamina && selectedItem.usable[i].usingType == UsingType.Gradual)
                {
                    StartCoroutine(CharacterManager.Instance.Player.condition.stamina.CoroutineAdd(selectedItem.usable[i].value, selectedItem.usable[i].time));
                }
            }

            RemoveSelectedItem();
        }
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equipment.EquipNew(selectedItem);
        UpdateUI();
        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equipment.UnEquip();
        UpdateUI();
        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    public void OnDropBtn()
    {
        ThrowItem(selectedItem);
    }

}
