using System;
using UnityEngine;



public class Unit : Selectable, IDamagable
{
    #region Actions

    public static Action<GameObject> UnitIsDead;
    public static Action<GameObject> UnitHealthChanged;
    public static Action<GameObject> HealthChanged;

    #endregion

    #region SerializedFields

    [SerializeField] protected UnitData unitData;
    [SerializeField] protected UIBar healthBar;
    [SerializeField] protected PlayerString owner = PlayerString.Undefined;
    [SerializeField] protected LayerMask unitLayer;
  
    #endregion

    #region PrivateFields

    protected bool humanConrolledUnit;
    protected Renderer[] meshRenderers;

    #endregion

    #region PublicFields

    public UnitData UnitData => unitData;
    public PlayerString Owner => owner;
    public bool HumanControlledUnit => humanConrolledUnit;

    #endregion

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

    protected override void StartSetup()
    {
        base.StartSetup();
        if (unitData != null)
        {
            currentHealth = unitData.HealthMax;
        }

        if (healthBar != null) healthBar.gameObject.SetActive(false);
        if (selectionCircle != null) selectionCircle.gameObject.SetActive(false);

        //UnitSelection.UnitOnSelection += ChangeSelectionVisibility;
        UnitSelection.UnitOnHover += ChangeHealthBarVisibility;

    }

    protected override void DeathSetup()
    {
        base.DeathSetup();
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

    protected override void Death()
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

    public override void OnSelect()
    {
        if(!humanConrolledUnit) return;
        base.OnSelect();

    }

}
