using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    private void Update()
    {
        if (CharacterManager.Instance.Player.condition.hunger.curValue <= 0)
            health.Substract(health.passiveValue * Time.deltaTime);
        if (health.curValue <= 0)
            Die();
        hunger.Substract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
    }
    private void Die()
    {

    }
}
