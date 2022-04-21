using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    Idle,
    MoveToPosition,
    MoveToTarget,
    Attack
}

public class Character : Unit
{
    #region Fields

    [SerializeField] private CharacterData data;
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private Animator anim;

    private float lastAttackTime = 0f;
    private Vector3 targetPos;
    private CharacterState state;

    public CharacterData Data => data;
    
    [Header("TEST")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject targetObj;


    #endregion

    #region UnityFunctions

    private void Update()
    {        
        switch (state)
        {
            case CharacterState.Idle:
                CheckDetectionRange();
                break;

            case CharacterState.MoveToTarget:
                break;

            case CharacterState.MoveToPosition:
                MoveToPosition();
                break;

            case CharacterState.Attack:               
                AttackCheck();
                CheckDetectionRange();
                break;

            default:
                break;
        }
    }

    #endregion

    #region Setup

    protected override void StartSetup()
    {
        base.StartSetup();
        state = CharacterState.Idle;
        if (navAgent) navAgent.speed = data.MovementSpeed;
        if (data != null)
        {
            currentHealth = data.HealthMax;
            InstaniateWeapon();          
        }        
    }

    protected override void SetUnitType()
    {
        unitType = data.UType;
    }

    private void InstaniateWeapon()
    {
        var wp = Instantiate(data.Weapon, weaponPosition);
        weapon = wp.GetComponent<Weapon>();
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(healthBar != null) healthBar.UpdateValue(currentHealth, data.HealthMax);
    }

    public void SetTarget(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer != gameObject.layer  
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground") 
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Environment"))
        {
            targetObj = hit.collider.gameObject;
            state = CharacterState.Attack;
        }
        else
        {
            targetPos = hit.point;
            state = CharacterState.MoveToPosition;
        }  
    }

    private void MoveToTarget()
    {
        LookAtTarget(targetObj.transform.position);
        navAgent.isStopped = false;
        navAgent.speed = data.MovementSpeed;
        navAgent.acceleration = data.MovementSpeed * 2;
        navAgent.stoppingDistance = 1f;
        navAgent.SetDestination(targetObj.transform.position);
    }

    public void MoveToPosition()
    {
        navAgent.isStopped = false;

        navAgent.SetDestination(targetPos);

        if (navAgent.remainingDistance < 0.1f)
        {
            state = CharacterState.Idle;
        }
    }

    private void CheckDetectionRange()
    {
        targetObj = null;

        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, data.ActionRange, unitLayer);
        float shortestDistance = 0f;

        if (targetsInRange.Length == 0) return;
        
        foreach (var target in targetsInRange)
        {
            var unit = target.gameObject.GetComponent<Unit>();

            if (unit.Owner == owner) continue;

            var distance = Distance(target.transform.position);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                targetObj = target.gameObject;
            }           
        }
     
        if (targetObj != null)
        {           
            state = CharacterState.Attack;
        }
        else
        {  
            state = CharacterState.Idle;
        }
    }

    private void AttackCheck()
    {
        if (targetObj == null) return;
        if (isDead) return;

        if (Distance(targetObj.transform.position) <= data.AttackRange)
        {
            LookAtTarget(targetObj.transform.position);
            navAgent.stoppingDistance = 2f;
            navAgent.isStopped = true;
            Attack();
        }
        else
        {
            MoveToTarget();           
        }
    }

    private void Attack()
    {
        if (lastAttackTime >= data.AttackSpeed)
        {
            lastAttackTime = 0f;
            if (weapon != null) weapon.Attack();
        }
        else
        {
            lastAttackTime += Time.deltaTime;
        }
    }

    private void LookAtTarget(Vector3 targetposition)
    {
        targetposition.y = transform.position.y;       
        transform.LookAt(targetposition);
    }

    private void StopNavAgent()
    {
        navAgent.velocity = Vector3.zero;
    }

    private float Distance(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    private void OnDrawGizmos()
    {
        if (data != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.AttackRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, data.ActionRange);
        }       
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponPosition != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(weaponPosition.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
        }       
    }
}