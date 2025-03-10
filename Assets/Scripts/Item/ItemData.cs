using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Usable,
    Resource,
}
public enum UsableType
{
    Health,
    Hunger,
    Stamina,
}
public enum UsingType
{
    Instant,
    Gradual
}
public enum StatType
{
    MoveSpeed,
    jumpPower
}
[Serializable]
public class ItemDataUsable
{
    public UsableType type;
    public UsingType usingType;
    public float value;
    public float time;
}
[Serializable]
public struct stat
{
    public StatType statType;
    public string value;
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
    public bool weapon;
    public GameObject equipPrefab;

    public stat[] stat;
}
