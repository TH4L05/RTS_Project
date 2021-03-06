/// <author> Thomas Krahl </author>

using UnityEngine;

[System.Serializable]
public class ResourceSetup
{
    public ResourceType resourceType;
    public int amount;

    public ResourceSetup()
    {
    }

    public ResourceSetup(ResourceType resourceType, int amount)
    {
        this.resourceType = resourceType;
        this.amount = amount;
    }
}


public enum UnitType
{
    Building,
    Character,
    //Hero
}

public class UnitData : ScriptableObject
{
    #region SerializedFields

    //[Header("UnitBase")]
    [SerializeField] private new string name;
    [SerializeField] private UnitType type;
    [SerializeField] private string tooltip;
    [SerializeField] private float buildTime;
    [SerializeField] private float deathTime;
    [SerializeField] private ResourceSetup[] requiredResources = new ResourceSetup[]
                                                                                    {
                                                                                    new ResourceSetup(ResourceType.Wood, 0),
                                                                                    new ResourceSetup(ResourceType.Gold, 0),
                                                                                    new ResourceSetup(ResourceType.Food, 0),
                                                                                    new ResourceSetup(ResourceType.Unit, 0)
                                                                                    };

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

    public virtual void SetDataFromStrings(string[] data)
    {
        name = data[0];
        tooltip = data[2];

        buildTime = float.Parse(data[3]);
        deathTime = float.Parse(data[4]);

        requiredResources[0].amount = int.Parse(data[5]);
        requiredResources[1].amount = int.Parse(data[6]);
        requiredResources[2].amount = int.Parse(data[7]);
        requiredResources[3].amount = int.Parse(data[8]);

        healthMax = float.Parse(data[9]);
        healthRegen = float.Parse(data[10]);
        healthRegenRate = float.Parse(data[11]);

        manaMax = float.Parse(data[12]);
        manaRegen = float.Parse(data[13]);
        manaRegenRate = float.Parse(data[14]);

        armor = float.Parse(data[15]);

        attackRange = float.Parse(data[16]);
        actionRange = float.Parse(data[17]);

        attackSpeed = int.Parse(data[18]);
        basedamage = int.Parse(data[19]);
           
        if (data[20] != "None")
        {
            var obj = Resources.Load("Weapons/" + data[20], typeof(GameObject));          
            if(obj != null) weapon = obj as GameObject;
        }

        for (int i = 0; i < abilities.Length; i++)
        {
            if (data[21+ i] == "None") continue;

            var obj = Resources.Load("Abilities/" + data[21 + i], typeof(ScriptableObject));
            if (obj != null) abilities[i] = obj as Ability;
        }
    }
}

