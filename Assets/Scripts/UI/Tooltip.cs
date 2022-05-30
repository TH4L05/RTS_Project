/// <author> Thomas Krahl </author>

using UnityEngine;
using TMPro;

[System.Serializable]
public class Tooltip : MonoBehaviour
{
    #region SerializedFields
 
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI descriptionField;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI text3;
    [SerializeField] private TextMeshProUGUI[] textFields;

    #endregion

    #region PrivateFields

    private static TextMeshProUGUI nameFieldStatic;
    private static TextMeshProUGUI descriptionFieldStatic;
    private static TextMeshProUGUI text2Static;
    private static TextMeshProUGUI text3Static;
    private static TextMeshProUGUI[] textFieldsStatic;
    private static GameObject obj;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        nameFieldStatic = nameField;
        descriptionFieldStatic = descriptionField;
        text2Static = text2;
        text3Static = text3;
        textFieldsStatic = textFields;

        obj = gameObject;
        obj.SetActive(false);
        ResetText();
    }

    #endregion

    public static void ResetText()
    {
        nameFieldStatic.text = "";
        descriptionFieldStatic.text = "";
        text2Static.text = "";
        text3Static.text = "";

        foreach (var textfield in textFieldsStatic)
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
            descriptionFieldStatic.text = "NO UNIT !!!";
            return;
        }

        var data = Utils.GetUnitData(unit);
        if (data == null) return;

        nameFieldStatic.text = data.name;
        descriptionFieldStatic.text = data.Tooltip;

        int resAmount = data.RequiredResources.Length;

        int index = 0;
        foreach (var resource in data.RequiredResources)
        {
            textFieldsStatic[index].text = resource.resourceType.ToString() + " : " + resource.amount.ToString();
            index++;
        }
    }

    public static void UpdateTooltip(string name, string description)
    {
        obj.SetActive(false);
        ResetText();
        obj.SetActive(true);

        nameFieldStatic.text = name;
        descriptionFieldStatic.text = description;
    }
}
