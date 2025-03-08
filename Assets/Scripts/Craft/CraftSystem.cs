using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftSystem : MonoBehaviour
{
    public CraftingRecipe recipe;
    public UIInventory inventory;

    public Button craftBtn;

    public void OnCraftBtn()
    {
        if (CanCraft(recipe))
            Craft(recipe);
        else
            Debug.Log("재료가 부족합니다.");
    }
    public void TestCraftBtn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (CanCraft(recipe))
                Craft(recipe);
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
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
}
