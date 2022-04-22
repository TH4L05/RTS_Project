using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Fields

    [Header("Base")]
    [SerializeField] protected float lifeTime = 5f;
    protected Rigidbody rbody;
    protected bool dectectHit;
    private WeaponData weaponData;
    private float speed;

    [Header("VFX/SFX")]
    [SerializeField] protected GameObject hitVFX;

    #endregion

    private void OnEnable()
    {
        rbody = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    public void Setup(WeaponData data)
    {
        weaponData = data;
        speed = weaponData.ProjectileSpeed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        dectectHit = true;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {          
            var unit = collider.gameObject.GetComponent<Unit>();
            var damage = weaponData.CalcDamage();
            unit.TakeDamage(damage);
        }
        Destroy(gameObject, 0.25f);

    }

    private void CalcDamage()
    {

    }

    void Update()
    {
        if (!dectectHit)
        {
            MoveProjectile();
        }
    }

    public virtual void MoveProjectile()
    {
        if (speed == 0f) speed = 10f;
 
        //rbody.AddForce(transform.forward * speed, ForceMode.Force);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (rbody != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, rbody.velocity, Time.deltaTime);
        }
    }
}
