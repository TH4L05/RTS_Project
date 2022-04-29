using System;
using UnityEngine;



public class Unit : MonoBehaviour, IDamagable, ISelectable
{
    #region Events

    public static Action<GameObject> UnitIsDead;
    public static Action<GameObject> HealthChanged;

    #endregion

    #region SerializedFields

    [SerializeField] protected UnitData unitData;
    [SerializeField] protected UIBar healthBar;
    [SerializeField] protected GameObject selectionCircle;
    [SerializeField] protected LayerMask unitLayer;
    [SerializeField] protected LayerMask groundLayer;

    #endregion

    #region PrivateFields

    protected PlayerString owner = PlayerString.Undefined;
    protected bool humanConrolledUnit;
    protected float currentHealth;
    protected float currentMana;
    protected bool isDead;
    protected Renderer[] meshRenderers;

    #endregion

    #region PublicFields

    public UnitData UnitData => unitData;
    public PlayerString Owner => owner;
    public bool HumanControlledUnit => humanConrolledUnit;
    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;


    #endregion

    #region UnityFunctions

    private void OnEnable()
    {
        StartSetup();
    }

    private void Start()
    {
        AdditionalSetup();
    }

    private void OnDestroy()
    {
        DeathSetup();
    }

    #endregion;


    #region Setup

    public virtual void SetOwner(PlayerString name, bool humanControlled)
    {
        owner = name;
        humanConrolledUnit = humanControlled;
    }

    public virtual void SetPlayerColor(Color color)
    {
        var model = transform.GetChild(0);
        meshRenderers = model.GetComponentsInChildren<Renderer>();

        foreach (var mr in meshRenderers)
        {
            foreach (var material in mr.materials)
            {
                material.color = color;
            }   
        }
    }

    protected virtual void StartSetup()
    {
        if (unitData != null)
        {
            currentHealth = unitData.HealthMax;
        }

        if (healthBar != null) healthBar.gameObject.SetActive(false);
        if (selectionCircle != null) selectionCircle.gameObject.SetActive(false);

        //UnitSelection.UnitOnSelection += ChangeSelectionVisibility;
        UnitSelection.UnitOnHover += ChangeHealthBarVisibility;

        unitLayer = 1 << gameObject.layer;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");

    }

    protected virtual void AdditionalSetup()
    {
    }

    protected virtual void DeathSetup()
    {
        //UnitSelection.UnitOnSelection -= ChangeSelectionVisibility;
        UnitSelection.UnitOnHover -= ChangeHealthBarVisibility;
    }

    #endregion

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log(name + "take Damage ");
        currentHealth -= damage;
        if (healthBar != null) healthBar.UpdateValue(currentHealth, unitData.HealthMax);
        HealthChanged?.Invoke(gameObject);
        
        if (currentHealth < 1)
        {
            isDead = true;
            Death();
        }
    }

    protected virtual void Death()
    {
        UnitIsDead?.Invoke(gameObject);
        Game.Instance.PlayerManager.RemoveUnit(this, owner);
        Destroy(gameObject, unitData.DeathTime);
    }

    #region Visuals

    public  virtual void ChangeHealthBarVisibility(Unit unit, bool visible)
    {
        if (healthBar == null) return;
        if (unit != this) return;
        healthBar.gameObject.SetActive(visible);
        healthBar.UpdateValue(currentHealth, unitData.HealthMax);
    }

    #endregion

    public virtual void OnSelect()
    {
        if(!humanConrolledUnit) return;
        ChangeSelectionVisibility(true);
    }

    public virtual void OnDeselect()
    {
        ChangeSelectionVisibility(false);
    }

    protected virtual void ChangeSelectionVisibility(bool visible)
    {
        if (selectionCircle != null) selectionCircle.SetActive(visible);
    }

    public void OnDeSelect()
    {
        throw new NotImplementedException();
    }
}
