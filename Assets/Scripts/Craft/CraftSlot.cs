using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    public CraftingRecipe recipe;
    public CraftSystem craftSystem;

    public int index;

    public void OnClickBtn()
    {
        craftSystem.recipe = recipe;
    }
}
