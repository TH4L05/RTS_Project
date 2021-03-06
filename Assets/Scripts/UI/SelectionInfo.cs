/// <author> Thomas Krahl </author>

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectionInfo : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI nameInfo;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI healthInfo;
    [SerializeField] private TextMeshProUGUI manaInfo;
    private GameObject selectedObj;

    #endregion

    #region UnityFunctions

    void Start()
    {
        UnitSelection.ObjectSelected += UpdateInfoOnSelect;
        UnitSelection.ObjectDeselected += UpdateInfoOnDeselect;
        Unit.HealthChanged += UpdateInfoHealth;
        UpdateInfoOnDeselect();
    }

    private void OnDestroy()
    {
        UnitSelection.ObjectSelected -= UpdateInfoOnSelect;
        UnitSelection.ObjectDeselected -= UpdateInfoOnDeselect;
        Unit.HealthChanged -= UpdateInfoHealth;
    }

    #endregion

    #region UpdateInfo

    void UpdateInfoOnSelect(GameObject obj)
    {
        selectedObj = obj;
        var unit = obj.GetComponent<Unit>();
        UnitData data = Utils.GetUnitData(unit);

        if (nameInfo != null) nameInfo.text = data.Name;
        if (healthInfo != null) healthInfo.text = $"{unit.CurrentHealth} / {data.HealthMax}";
        if (manaInfo != null) manaInfo.text = $"{unit.CurrentMana} / {data.ManaMax}";

        if (icon != null)
        {
            icon.sprite = data.SelectionInfoIcon;
            icon.gameObject.SetActive(true);
        }


    }
   
    void UpdateInfoOnDeselect()
    {
        selectedObj = null;
        if (nameInfo != null) nameInfo.text = "";
        if (healthInfo != null) healthInfo.text = "";
        if (manaInfo != null) manaInfo.text = "";

        if (icon != null)
        {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
        }            
    }

    void UpdateInfoHealth(GameObject obj)
    {
        if (obj != selectedObj) return;
        var unit = obj.GetComponent<Unit>();
        UnitData data = Utils.GetUnitData(unit);
        if (healthInfo != null) healthInfo.text = $"?{unit.CurrentHealth} / {data.HealthMax}";
    }

    #endregion
}
