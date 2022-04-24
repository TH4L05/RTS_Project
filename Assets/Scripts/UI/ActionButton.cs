using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ActionButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Button button;
    [SerializeField] protected Image icon;
    private Sprite defaultSprite;
    private Unit unit;
    private int index;

    void Start()
    {
        defaultSprite = icon.sprite;
    }

    public virtual void SetAction(GameObject obj,int gridIndex)
    {
        var unit = obj.GetComponent<Unit>();
        this.unit = unit;
        index = gridIndex;

        UnitData data = Utils.GetUnitData(unit); 
        
        if (data.Abilities[gridIndex] == null) return;
    
        button.interactable = true;

        if (!data.Abilities[gridIndex].UseTemplateSprites)
        {
            SetIcons(
                data.Abilities[gridIndex].Icon, 
                data.Abilities[gridIndex].IconHighlighted, 
                data.Abilities[gridIndex].IconPressed, 
                data.Abilities[gridIndex].IconDisabled);       
        }
        else
        {
            SetIcons(data.Abilities[gridIndex].UnitTemplate.GetComponent<Unit>());
        }

        button.onClick.AddListener(delegate { data.Abilities[gridIndex].DoAction(obj); });
    }
     
    protected virtual void SetIcons(Sprite normalSprite, Sprite higlightedSprite, Sprite pressedSprite, Sprite disabledSprite)
    {
        icon.sprite = normalSprite;
        var spriteState = new SpriteState();
        spriteState.pressedSprite = higlightedSprite;
        spriteState.highlightedSprite = higlightedSprite;
        spriteState.disabledSprite = disabledSprite;

        button.spriteState = spriteState;
    }

    protected virtual void SetIcons(Unit unit)
    {
        UnitData data = Utils.GetUnitData(unit);
        icon.sprite = data.ActionButtonIcon;
        var spriteState = new SpriteState();
        spriteState.pressedSprite = data.ActionButtonIconPressed;
        spriteState.highlightedSprite = data.ActionButtonIconHighlighted;

        button.spriteState = spriteState;
    }

    private void ResetIcons()
    {
        icon.sprite = defaultSprite;
        var spriteState = new SpriteState();
        spriteState.pressedSprite = null;
        spriteState.highlightedSprite = null;
        spriteState.disabledSprite = null;
        button.spriteState = spriteState;
    }

    public virtual void RemoveAction()
    {
        button.onClick.RemoveAllListeners();
        button.interactable = false;
        ResetIcons();
        unit = null;
        index = -1;
    }

    protected virtual void ShowTooltip()
    {
        if(unit == null) return;
        if (!button.interactable) return;

        Game.Instance.TooltipUI.ResetText();
        Game.Instance.TooltipUI.gameObject.SetActive(true);
        UnitData data = Utils.GetUnitData(unit);

        if (data.Abilities[index].UnitTemplate == null)
        {
            Game.Instance.TooltipUI.UpdateTooltip(data.Abilities[index].Tooltip);
        }
        else
        {
            Unit tunit = data.Abilities[index].UnitTemplate.GetComponent<Unit>();
            Game.Instance.TooltipUI.UpdateTooltip(tunit);
        }
    }

    protected virtual void HideTooltip()
    {
        Game.Instance.TooltipUI.gameObject.SetActive(false);
        Game.Instance.TooltipUI.ResetText();
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
}
