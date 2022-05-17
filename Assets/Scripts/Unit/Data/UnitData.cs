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
    Undefined = -1,
    Building,
    Character
}

public class UnitData : ScriptableObject
{
    #region SerializedFields

    //[Header("UnitBase")]
    [SerializeField] private new string name;
    [SerializeField] private UnitType type = UnitType.Undefined;
    [SerializeField] private string tooltip;
    [SerializeField] private float buildTime;
    [SerializeField] private float deathTime;
    [SerializeField] private ResourceSetup[] requiredResources;

    //[Header("Stats")]
    [SerializeField] private float healthMax = 1f;
    [SerializeField] private float healthRegen;
    [SerializeField] private float healthRegenRate;
    [SerializeField] private float manaMax;
    [SerializeField] private float manaRegen;
    [SerializeField] private float manaRegenRate;
    [SerializeField] private float armor;

    //[Header("Fighting")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float actionRange = 2f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float basedamage = 1f;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float weaponAttackOffsetTime;

    //[Header("Abilities")]
    [SerializeField] private Ability[] abilities = new Ability[12];

    //[Header("Visuals")]
    [SerializeField] private Sprite selectionInfoIcon;
    [SerializeField] private Sprite actionButtonIcon;
    [SerializeField] private Sprite actionButtonIconHighlighted;
    [SerializeField] private Sprite actionButtonIconPressed;

    #endregion

    #region PublicFields

    public string Name => name;
    public UnitType Type => type;
    public string Tooltip => tooltip;
    public ResourceSetup[] RequiredResources => requiredResources;
    public float BuildTime => buildTime;
    public float DeathTime => deathTime;
    public float HealthMax => healthMax;
    public float HealthRegen => healthRegen;
    public float ManaMax => manaMax;
    public float ManaRegen => manaRegen;
    public float Armor => armor;
    public float AttackRange => attackRange;
    public float ActionRange => actionRange;
    public float AttackSpeed => attackSpeed;
    public float Damage => basedamage;
    public GameObject Weapon => weapon;
    public float WeaponAttackOffsetTime => weaponAttackOffsetTime;
    public Ability[] Abilities => abilities;
    public Sprite SelectionInfoIcon => selectionInfoIcon;
    public Sprite ActionButtonIcon => actionButtonIcon;
    public Sprite ActionButtonIconHighlighted => actionButtonIconHighlighted;
    public Sprite ActionButtonIconPressed => actionButtonIconPressed;

    #endregion

    public void SetTypeAndName(UnitType type, string name)
    {
        this.name = name;
        this.type = type;
        tooltip = this.type.ToString();
    }

    public void SetDataFromStrings(string[] data)
    {
        name = data[0];
        tooltip = data[2];
        buildTime = float.Parse(data[3]);
        deathTime = float.Parse(data[4]);
        healthMax = float.Parse(data[4]);
        healthRegen = float.Parse(data[6]);
        healthRegenRate = float.Parse(data[7]);
        manaMax = float.Parse(data[8]);
        manaRegen = float.Parse(data[9]);
        manaRegenRate = float.Parse(data[10]);
        //armor = float.Parse(data[11]);
    }
}
