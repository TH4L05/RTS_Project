using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI text3;

    [SerializeField] private TextMeshProUGUI[] textFields;

    private void Awake()
    {
        ResetText();
    }

    public void ResetText()
    {
        description.text = "";
        text2.text = "";
        text3.text = "";

        foreach (var textfield in textFields)
        {
            textfield.text = "";
        }
    }

    public void UpdateTooltip(Unit unit)
    {
        ResetText();

        if (unit == null)
        {
            description.text = "NO UNIT !!!";
            return;
        }

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

        description.text = data.Tooltip;

        int resAmount = data.RequiredResources.Length;

        int index = 0;
        foreach (var resource in data.RequiredResources)
        {
            textFields[index].text = resource.ResoureData.Type.ToString() + " : " + resource.amount.ToString("000");
            index++;
        }
    }

    public void UpdateTooltip(string text)
    {
        ResetText();
        description.text = text;

    }
}
