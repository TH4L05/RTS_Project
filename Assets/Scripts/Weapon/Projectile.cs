/// <author> Thomas Krahl </author>

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

    #region UnityFunctions

    private void OnEnable()
    {
        rbody = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
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

    void Update()
    {
        if (!dectectHit)
        {
            MoveProjectile();
        }
    }

    #endregion

    public void Setup(WeaponData data)
    {
        weaponData = data;
        speed = weaponData.ProjectileSpeed;
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
