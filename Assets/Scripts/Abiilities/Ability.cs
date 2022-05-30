/// <author> Thomas Krahl </author>

using UnityEngine;

public class Ability : ScriptableObject
{
    #region Actions



    #endregion

    #region SerializedFields

    [SerializeField] protected bool editValues = false;
    [SerializeField] protected new string name;
    [SerializeField] protected string tooltip;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected Sprite iconHighlighted;
    [SerializeField] protected Sprite iconPressed;
    [SerializeField] protected Sprite iconDisabled;
    [SerializeField] protected GameObject unitTemplate;
    [SerializeField] protected bool useTemplateSprites;

    #endregion

    #region PrivateFields



    #endregion

    #region PublicFields
    public bool EditValues => editValues;
    public string Name => name;
    public string Tooltip => tooltip;
    public Sprite Icon => icon;
    public Sprite IconHighlighted => iconHighlighted;
    public Sprite IconPressed => iconPressed;
    public Sprite IconDisabled => iconDisabled;
    public GameObject UnitTemplate => unitTemplate;
    public bool UseTemplateSprites => useTemplateSprites;

    #endregion

    #region UnityFunctions
    #endregion

    public virtual void DoAction(GameObject obj)
    {
        Game.Instance.Unitselection.ShortPause();
    }
}