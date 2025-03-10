using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    float baseSpeed;
    float baseDashSpeed;
    float baseJumpPower;

    public Equip curEquip;
    public Transform equipParent;

    private void Start()
    {
        baseSpeed = CharacterManager.Instance.Player.controller.moveSpeed;
        baseJumpPower = CharacterManager.Instance.Player.controller.jumpPower;
    }
    public void EquipNew(ItemData data)
    {
        UnEquip();
        if (data.weapon)
            curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
        else
            curEquip = Instantiate(data.equipPrefab, CharacterManager.Instance.Player.transform).GetComponent<Equip>();

        foreach (var stat in data.stat)
        {
            if (float.TryParse(stat.value, out float statValue))
            {
                ApplyStat(stat.statType, statValue);
            }
        }

    }
    public void UnEquip()
    {
        if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;

            CharacterManager.Instance.Player.controller.curSpeed = baseSpeed;
            CharacterManager.Instance.Player.controller.moveSpeed = CharacterManager.Instance.Player.controller.curSpeed;
            CharacterManager.Instance.Player.controller.jumpPower = baseJumpPower;
        }
    }

    public void ApplyStat(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MoveSpeed:
                CharacterManager.Instance.Player.controller.curSpeed += value;
                CharacterManager.Instance.Player.controller.moveSpeed = CharacterManager.Instance.Player.controller.curSpeed;
                break;
            case StatType.jumpPower:
                CharacterManager.Instance.Player.controller.jumpPower += value;
                break;
        }
    }
}
