using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonUnit : ActionButton
{
    public override void SetAction(Unit unit, int gridIndex)
    {
        this.unit = unit;
        go = unit.gameObject;

        button.interactable = true;
        button.onClick.AddListener(delegate { Game.Instance.BuildMode.ActivateMode(go); });

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

        SetIcon(data.ActionButtonIcon);
    }

    protected override void ShowTooltip()
    {
        if (button.interactable)
        {
            Game.Instance.tooltipUI.transform.gameObject.SetActive(true);
            Game.Instance.tooltipUI.UpdateTooltip(unit);
        }
    }

    protected override void HideTooltip()
    {
        if (button.interactable)
        {
            Game.Instance.tooltipUI.transform.gameObject.SetActive(true);
            Game.Instance.tooltipUI.ResetText();
        }
    }
}
