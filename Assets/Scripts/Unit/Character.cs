/// <author> Thomas Krahl </author>

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
    #region Actions
    #endregion


    #region SerializedFields

    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private Animator anim;
    [SerializeField] private bool canAttack = false;

    #endregion

    #region PrivateFields

    private CharacterData data => unitData as CharacterData;
    private Weapon weapon;
    private float lastAttackTime = 0f;
    private float speed;
    private Vector3 targetPos;
    private CharacterState state;
    private CharacterState lastState;
    private GameObject targetObj;

    #endregion

    #region PublicFields
    #endregion

    #region UnityFunctions

    private void Update()
    {
        if (isDead) return;
        UpdateState();
    }

    #endregion

    #region Setup

    protected override void StartSetup()
    {
        base.StartSetup();
        state = CharacterState.Idle;
        if (navAgent) navAgent.speed = data.MovementSpeed;       
    }

    protected override void AdditionalSetup()
    {
        if (data != null)
        {
            InstaniateWeapon();
        }        
    }

    private void InstaniateWeapon()
    {
        if (data.Weapon == null) return;
        var wp = Instantiate(data.Weapon, weaponPosition);
        weapon = wp.GetComponent<Weapon>();
        weapon.Setup(anim, owner, unitLayer);
    }

    #endregion

    private void UpdateState()
    {
        speed = navAgent.velocity.magnitude / navAgent.speed;

        switch (state)
        {
            case CharacterState.Idle:
                if (anim != null) anim.SetFloat("speed", 0f);
                if (canAttack)
                {
                    CheckDetectionRange();
                }
                break;

            case CharacterState.MoveToTarget:
                break;

            case CharacterState.MoveToPosition:
                if (anim != null) anim.SetFloat("speed", speed);
                MoveToPosition();
                break;

            case CharacterState.Attack:
                AttackCheck();
                CheckDetectionRange();
                break;

            default:
                break;
        }

        lastState = state;

    }

    private void MoveToTarget()
    {
        if (anim != null) anim.SetFloat("speed", speed);
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

        if (navAgent.remainingDistance < 1f)
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

            var distance = Utils.GetDistance(transform.position,target.transform.position);

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
            state = lastState;
        }
    }

    public void SetTarget(GameObject obj, RaycastHit hit)
    {    
        if (obj.layer == unitLayer)  
        {
            var unit = obj.GetComponent<Unit>();

            if (unit.Owner != owner && canAttack)
            {
                targetObj = obj;
                state = CharacterState.Attack;
                return;
            }
        }
        targetPos = hit.point;
        state = CharacterState.MoveToPosition;      
    }

    public void SetTarget(Vector3 position)
    {
        targetPos = position;
        state = CharacterState.MoveToPosition;
    }


    private void AttackCheck()
    {
        if (targetObj == null) return;
        if (isDead) return;

        var distance = Utils.GetDistance(transform.position, targetObj.transform.position);

        if (distance <= data.AttackRange)
        {
            LookAtTarget(targetObj.transform.position);
            navAgent.stoppingDistance = 2f;
            navAgent.isStopped = true;
            anim.SetFloat("speed", 0f);
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
            if (weapon != null)
            {
                if (anim != null) anim.SetTrigger("Attack");
                weapon.Attack();
            }
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

    protected override void Death()
    {
        base.Death();
        if (anim != null) anim.SetBool("dead", true);
        if (anim != null) anim.SetTrigger("death");
    }

    #region Gizmos

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

    #endregion
}