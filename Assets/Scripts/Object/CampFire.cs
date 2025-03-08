using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    List<Condition> character = new();

    private void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }
    void DealDamage()
    {
        for(int i  = 0; i < character.Count; i++)
        {
            character[i].TakeOnDamage(damage);
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Condition condition))
            character.Add(condition);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Condition condition))
            character.Remove(condition);
    }
}
