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
    //[SerializeField] protected UnitType unitType = UnitType.Undefined;
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

    protected override void StartSetup()
    {
        base.StartSetup();
        if (unitData != null)
        {
            currentHealth = unitData.HealthMax;
        }
        ChangeHealthBarVisibility(false);
        ChangeSelectionCircleVisibility(false);
    }

    #endregion

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log(name + "take Damage ");
        currentHealth -= damage;
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
    }

    #region Visuals

    public  virtual void ChangeHealthBarVisibility(bool visible)
    {
        if (healthBar == null) return;
        healthBar.gameObject.SetActive(visible);
        healthBar.UpdateValue(currentHealth, unitData.HealthMax);
    }

    public void ChangeSelectionCircleVisibility(bool visible)
    {
        if (selectionCircle != null) selectionCircle.gameObject.SetActive(visible);
    }

    #endregion

}
