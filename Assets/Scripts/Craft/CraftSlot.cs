using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    public CraftingRecipe recipe;
    public Image icon;
    public CraftSystem craftSystem;

    public int index;

    public void OnClickBtn()
    {
        craftSystem.index = index;
    }
}
