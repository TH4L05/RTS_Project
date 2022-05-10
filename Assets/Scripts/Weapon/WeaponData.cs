using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Undifned = -1,
    Normal,
    Projectile,

}

public enum DamageType
{
    Undifned = -1,
    Normal,
    Heavy,
}

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Base")]
    [SerializeField] private AttackType attackType = AttackType.Normal;
    
    [Header("Damage")]
    [SerializeField] private DamageType type;
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private int diceAmount = 1;
    [SerializeField] private int sidesPerDie = 1;

    [Header("Normal")]
    [SerializeField] private float damageTime;

    [Header("Projectile")]
    [SerializeField] private GameObject projectileTemplate;
    [SerializeField] private float projectileSpeed;

    public AttackType AttackType => attackType;
    public DamageType DamageType => type;
    public float BaseDamage => baseDamage;
    public GameObject ProjectileTemplate => projectileTemplate;
    public float ProjectileSpeed => projectileSpeed;
    public float DamageTime => damageTime;

    public float CalcDamage()
    {
        var damage = baseDamage;

        for (int i = 0; i < diceAmount; i++)
        {
            damage += Random.Range(1, sidesPerDie);
        }

        return damage;
    }
}
