using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActionButton: MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    public void SetAction(Unit unit,int gridIndex)
    {
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

        data.Abilities[gridIndex].SetObject(unit.gameObject);
        button.interactable = true;
        SetIcon(data.Abilities[gridIndex].Icon);
        button.onClick.AddListener(data.Abilities[gridIndex].DoAction);
    }
     
    private void SetIcon(Sprite sprite)
    {
        if (sprite == null) return;
        if (icon == null) return;
        icon.sprite = sprite;
    }

    public void RemoveAction()
    {
        button.onClick.RemoveAllListeners();
        button.interactable = false;
        icon.sprite = null;
    }
}
