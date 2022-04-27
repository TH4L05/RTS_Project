using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SelectionType
{
    Undefined = -1,
    Unit,
    Destructable,
    Item
}

public class Selectable : MonoBehaviour
{
    #region Actions
    #endregion

    #region SerializedFields

    [SerializeField] private SelectionType selectionType = SelectionType.Undefined;
    [SerializeField] protected GameObject selectionCircle;

    #endregion

    #region PrivateFields

    protected float currentHealth;
    protected float currentMana;
    protected bool isDead;

    #endregion

    #region PublicFields
    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;
    public SelectionType SselectionType => selectionType;

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

    protected virtual void StartSetup()
    {
    }

    protected virtual void AdditionalSetup()
    {
    }

    protected virtual void DeathSetup()
    {
    }

    protected virtual void Death()
    {
    }

    #endregion
}
