using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceSetup
{
    public ResourceData ResoureData;
    public int amount;
}


public enum UnitType
{
    Building,
    Character
}

public class UnitData : ScriptableObject
{
    #region SerializedFields

    [Header("Base")]
    [SerializeField] private new string name;
    [SerializeField] private string tooltip;
    [SerializeField] private UnitType uType;
    [SerializeField] private float buildTime;
    [SerializeField] private ResourceSetup[] requiredResources;

    [Header("Stats")]
    [SerializeField] private float healthMax = 1f;
    [SerializeField] private float healthRegen;
    [SerializeField] private float manaMax;
    [SerializeField] private float manaRegen;
    [SerializeField] private float amor;

    [Header("Fighting")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float actionRange = 2f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float basedamage = 1f;
    [SerializeField] private GameObject weapon;

    [Header("Other")]
    [SerializeField] private GameObject[] producableUnits;
    [SerializeField] private Ability[] abilities = new Ability[12];

    [Header("Visuals")]
    [SerializeField] private Sprite selectionInfoIcon;
    [SerializeField] private Sprite actionButtonIcon;
    [SerializeField] private Sprite actionButtonIconHighlighted;
    [SerializeField] private Sprite actionButtonIconPressed;

    #endregion

    #region PublicFields

    public string Name => name;
    public string Tooltip => tooltip;
    public UnitType UType => uType;
    public ResourceSetup[] RequiredResources => requiredResources;
    public float BuildTime => buildTime;
    public float HealthMax => healthMax;
    public float HealthRegen => healthRegen;
    public float ManaMax => manaMax;
    public float ManaRegen => manaRegen;
    public float Amor => amor;
    public float AttackRange => attackRange;
    public float ActionRange => actionRange;
    public float AttackSpeed => attackSpeed;
    public float Damage => basedamage;
    public GameObject Weapon => weapon;
    public GameObject[] ProducableUnits => producableUnits;
    public Ability[] Abilities => abilities;
    public Sprite SelectionInfoIcon => selectionInfoIcon;
    public Sprite ActionButtonIcon => actionButtonIcon;
    public Sprite ActionButtonIconHighlighted => actionButtonIconHighlighted;
    public Sprite ActionButtonIconPressed => actionButtonIconPressed;

    #endregion
}
