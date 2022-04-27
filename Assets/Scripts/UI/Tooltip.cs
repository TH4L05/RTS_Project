using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private static TextMeshProUGUI nameField;
    [SerializeField] private static TextMeshProUGUI descriptionField;
    [SerializeField] private static TextMeshProUGUI text2;
    [SerializeField] private static TextMeshProUGUI text3;

    [SerializeField] private static TextMeshProUGUI[] textFields;

    private static GameObject obj;

    private void Awake()
    {
        obj = gameObject;
        ResetText();
    }

    public static void ResetText()
    {       
        nameField.text = "";
        descriptionField.text = "";
        text2.text = "";
        text3.text = "";

        foreach (var textfield in textFields)
        {
            textfield.text = "";
        }
    }

    public static void ShowTooltip(bool visible)
    {
        if (visible)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(false);
        }
    }

    public static void UpdateTooltip(Unit unit)
    {
        obj.SetActive(false);
        ResetText();
        obj.SetActive(true);


        if (unit == null)
        {
            descriptionField.text = "NO UNIT !!!";
            return;
        }

        var data = Utils.GetUnitData(unit);
        if (data == null) return;

        nameField.text = data.name;
        descriptionField.text = data.Tooltip;

        int resAmount = data.RequiredResources.Length;

        int index = 0;
        foreach (var resource in data.RequiredResources)
        {
            textFields[index].text = resource.ResoureData.Type.ToString() + " : " + resource.amount.ToString();
            index++;
        }
    }

    public static void UpdateTooltip(string name, string description)
    {
        ResetText();
        nameField.text = name;
        descriptionField.text = description;
    }
}
