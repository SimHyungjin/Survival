using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftSystem : MonoBehaviour
{
    public CraftingRecipe[] recipe;
    public UIInventory inventory;
    public CraftSlot[] slots;
    public Transform slotPanel;
    public Button craftBtn;
    public Sprite emptyImage;

    public int index;

    private void Start()
    {
        slots = new CraftSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<CraftSlot>();
            slots[i].icon = slotPanel.GetChild(i).GetComponent<Image>();
            slots[i].icon.gameObject.SetActive(false);
            slots[i].recipe = null;
            slots[i].index = i;
            slots[i].craftSystem = this;
        }
        inventory.addItem += UploadRecipe;
        CharacterManager.Instance.Player.dropItem += UploadRecipe;
    }

    CraftSlot CheckUseSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].recipe == null)
                return slots[i];
        }
        return null;
    }

    public void UploadRecipe()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].recipe = null;
            slots[i].icon.gameObject.SetActive(false);
        }
        for (int i = 0; i < recipe.Length; i++)
        {
            if (CanCraft(recipe[i]))
            {
                CraftSlot slot = CheckUseSlot();
                if (slot != null)
                {
                    slot.icon.gameObject.SetActive(true);
                    slot.recipe = recipe[i];
                    slot.icon.sprite = recipe[i].icon;
                }
            }
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        if (recipe == null)
            return false;
        for (int i = 0; i < recipe.materials.Length; i++)
        {
            ItemData requiredItem = recipe.materials[i].item;
            int requiredCount = recipe.materials[i].count;
            int inventoryCount = inventory.GetItemCount(requiredItem);

            if (requiredCount > inventoryCount)
                return false;
        }
        return true;
    }

    void Craft(CraftingRecipe recipe)
    {
        for (int i = 0; i < recipe.materials.Length; i++)
        {
            inventory.RemoveItemNum(recipe.materials[i].item, recipe.materials[i].count);
        }
        Instantiate(recipe.resultItem, CharacterManager.Instance.Player.makePos.position, Quaternion.identity);
    }

    public void OnCraftBtn()
    {
        if (CanCraft(slots[index].recipe))
        {
            Craft(slots[index].recipe);
            UploadRecipe();
        }
        else
            return;
    }
}
