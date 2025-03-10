using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "New Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Serializable]
    public struct MaterialRequirement
    {
        public ItemData item;
        public int count;
    }
    public MaterialRequirement[] materials;

    public Sprite icon;
    public GameObject resultItem;
}