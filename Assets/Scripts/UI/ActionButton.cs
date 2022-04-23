using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ActionButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Button button;
    [SerializeField] protected Image icon;
    protected GameObject go;
    protected Unit unit;

    public virtual void SetAction(Unit unit,int gridIndex)
    {
        this.unit = unit;
        go = unit.gameObject;

        UnitData data = null;
        UnitType type = unit.UnitType;

        switch (type)
        {
            case UnitType.Building:
                data = unit.GetComponent<Building>().Data;
                break;
            case UnitType.Character:
                data = unit.GetComponent<Character>().Data;
                break;
            default:
                break;
        }

        if (data.Abilities[gridIndex] == null) return;

        Debug.Log(unit.gameObject.name);
        data.Abilities[gridIndex].SetObject(unit.gameObject);
        button.interactable = true;
        SetIcon(data.Abilities[gridIndex].Icon);
        button.onClick.AddListener(data.Abilities[gridIndex].DoAction);
    }
     
    protected virtual void SetIcon(Sprite sprite)
    {
        if (sprite == null) return;
        if (icon == null) return;
        icon.sprite = sprite;
    }

    public virtual void RemoveAction()
    {
        button.onClick.RemoveAllListeners();
        button.interactable = false;
        icon.sprite = null;
    }

    protected virtual void ShowTooltip()
    {
        Game.Instance.tooltipUI.gameObject.SetActive(true);
        //Game.Instance.tooltipUI.UpdateTooltip(null);
    }

    protected virtual void HideTooltip()
    {
        Game.Instance.tooltipUI.gameObject.SetActive(false);
        Game.Instance.tooltipUI.ResetText();
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
