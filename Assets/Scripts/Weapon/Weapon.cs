using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : MonoBehaviour
{
    #region Fíelds

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector3 attackPointSize = Vector3.one;
    [SerializeField] private Animator anim;
    private float damage;


    #endregion

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

        if (anim == null)
        {
            Debug.LogError("NO Animator assigned !!");
            return;
        }
        anim.Play("Attack");
    }


    private void NormalAttack()
    {
        anim.SetBool("attack", false);
        Collider[] targets = Physics.OverlapBox(attackPoint.position, attackPointSize/2);

        foreach (var target in targets)
        {
            var unit = target.GetComponent<Unit>();
            if (unit.name != transform.parent.name)
            {
                unit.TakeDamage(damage);
            }                     
        }       
    }

    private void ProjectileAttack()
    {
        anim.SetBool("attack", false);
        var projectile = Instantiate(weaponData.ProjectileTemplate, attackPoint.position, attackPoint.rotation);
        projectile.GetComponent<Projectile>().Setup(weaponData);
    }

    #endregion

    private void OnDrawGizmos()
    {       
        if (attackPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(attackPoint.position, attackPointSize);
        }       
    } 
}
