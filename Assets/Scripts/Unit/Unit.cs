using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    #region Actions

    public static Action<GameObject> UnitIsDead;

    #endregion

    #region SerializedFields

    [SerializeField] protected UIBar healthBar;
    [SerializeField] protected GameObject selectionCircle;
    [SerializeField] protected PlayerString owner = PlayerString.Undefined;
    [SerializeField] protected UnitType unitType = UnitType.Undefined;
    [SerializeField] protected LayerMask unitLayer;

    #endregion

    #region PrivateFields

    protected float currentHealth;
    protected float currentMana;
    protected bool isDead;
    protected bool humanConrolledUnit;
    protected Renderer[] meshRenderers;

    #endregion

    #region PublicFields

    public PlayerString Owner => owner;

    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;
    public UnitType UnitType => unitType;
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

    private void OnMouseOver()
    {
        //Debug.Log("MouseOver");
        ChangeHealthBarVisibility(true);
    }

    private void OnMouseExit()
    {
        ChangeHealthBarVisibility(false);
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
        //if (meshRenderers.Length == 0) return;

        foreach (var mr in meshRenderers)
        {
            mr.material.color = color;
        } 
    }

    protected virtual void StartSetup()
    {
        var model = transform.GetChild(0);
        meshRenderers = model.GetComponentsInChildren<Renderer>();

        ChangeHealthBarVisibility(false);
        ChangeSelectionCircleVisibility(false);
    }

    protected virtual void AdditionalSetup()
    {
    }

    protected virtual void DeathSetup()
    {
    }

    #endregion

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
        Game.Instance.PlayerManager.RemoveUnit(this, owner);       
    }


    #region Visuals

    public void ChangeHealthBarVisibility(bool visible)
    {
        if (healthBar != null) healthBar.gameObject.SetActive(visible);       
    }

    public void ChangeSelectionCircleVisibility(bool visible)
    {
        if (selectionCircle != null) selectionCircle.gameObject.SetActive(visible);
    }

    #endregion

}
