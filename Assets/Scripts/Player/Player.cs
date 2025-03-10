using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public Equipment equipment;
    public Animator animator;
    public Equip equip;

    public ItemData itemData;

    public Action addItem;
    public Action dropItem;

    public Transform dropPos;
    public Transform makePos;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        animator = GetComponentInChildren<Animator>();
        equipment = GetComponent<Equipment>();

    }
    
}
