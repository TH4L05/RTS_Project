using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Fíelds

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private Collider coll;

    private float damage;
    private PlayerString owner;

    #endregion

    public void Setup(Animator anim, PlayerString p, LayerMask unitLayer)
    {
        owner = p;
        this.unitLayer = unitLayer;
    }

    #region Damage

    private void SetDamage()
    {
        if (weaponData == null)
        {
            Debug.LogError("NO WeaponData assigned !!!");
            damage = 1f;
            return;
        }

        damage = weaponData.CalcDamage();      
    }

    #endregion

    #region Attack

    public void Attack()
    {
        SetDamage();
        Debug.Log("Projectile");

        switch (weaponData.AttackType)
        {
            case AttackType.Undifned:
                break;
            case AttackType.Normal:
                break;
            case AttackType.Projectile:
                Debug.Log("Projectile");
                ProjectileAttack();
                break;
            default:
                break;
        }          
    }

    private void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponent<Unit>();
        if (unit.Owner == owner) return;
        Debug.Log(other.gameObject.name + "takes Damage");
        unit.TakeDamage(damage);     
    }

    private void ProjectileAttack()
    {
        var projectile = Instantiate(weaponData.ProjectileTemplate, projectileSpawn.position, projectileSpawn.rotation);
        projectile.GetComponent<Projectile>().Setup(weaponData);
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (projectileSpawn != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(projectileSpawn.position, new Vector3(0.25f, 0.25f, 0.25f));
        }    
    } 
}
