using System;
using UnityEngine;

public class Unit : MonoBehaviour
{  
    protected float currentHealth;
    protected float currentMana;
    protected bool isDead;

    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;

    [SerializeField] protected UIBar healthBar;
    [SerializeField] protected GameObject selectionCircle;
    [SerializeField] protected PlayerString owner = PlayerString.Undefined;
    [SerializeField] protected LayerMask unitLayer;

    public PlayerString Owner => owner;
    public static Action<GameObject> UnitIsDead;

    protected UnitType unitType;
    public UnitType UnitType => unitType;

    private void OnEnable()
    {
        StartSetup();
    }

    private void Start()
    {
        AdditionalSetup();
    }


    public void SetOwner(PlayerString name)
    {
        owner = name;
    }

    protected virtual void StartSetup()
    {
        SetUnitType();
        ChangeHealthBarVisibility(false);
        ChangeSelectionCircleVisibility(false);
    }

    protected virtual void SetUnitType()
    {
    }

    protected virtual void AdditionalSetup()
    {
    }

    protected virtual void DeathSetup()
    {
    }
 
    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log(name + "take Damage ");
        currentHealth -= damage;
        
        if (currentHealth < 1)
        {
            isDead = true;
            Death();
        }
    }

    protected virtual void Death()
    {
        UnitIsDead?.Invoke(gameObject);
        Game.Instance.RemovePlayerUnit(this,owner);
        Destroy(gameObject, 1f);
    }

    public void ChangeHealthBarVisibility(bool visible)
    {
        if (healthBar != null) healthBar.gameObject.SetActive(visible);       
    }

    public void ChangeSelectionCircleVisibility(bool visible)
    {
        if (selectionCircle != null) selectionCircle.gameObject.SetActive(visible);
    }
}
