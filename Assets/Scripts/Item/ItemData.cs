using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Usable,
    Resource
}
public enum UsableType
{
    Health,
    Hunger,
    Stamina,
    Stat
}

[Serializable]
public class ItemDataUsable
{
    public UsableType type;
    public float value;
}

[CreateAssetMenu(fileName ="Item",menuName ="New Item")]
public class ItemData : ScriptableObject
{
    [Header("info")]
    public string ItemName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int stackCount;

    [Header("Useable")]
    public ItemDataUsable[] usable;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Craft Info")]
    public bool canBeCrafted;
    public CraftingRecipe[] craftingRecipes;
}
