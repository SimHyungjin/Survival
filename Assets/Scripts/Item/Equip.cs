using UnityEngine;

public class Equip : MonoBehaviour
{
    public float attackRate;
    public float attackDistance;

    [Header("Rsource Gathering")]
    public bool doesGatherResources;
    public int doesGatherResourcesNum;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;
}
