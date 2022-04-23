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

    #endregion

    #region UnityFunctions

    void Start()
    {
        SelectionHandler.ObjectSelected += UpdateInfoOnSelect;
        SelectionHandler.ObjectDeselected += UpdateInfoOnDeselect;
        UpdateInfoOnDeselect();
    }

    private void OnDestroy()
    {
        SelectionHandler.ObjectSelected -= UpdateInfoOnSelect;
        SelectionHandler.ObjectDeselected -= UpdateInfoOnDeselect;
    }

    #endregion

    #region UpdateInfo

    void UpdateInfoOnSelect(GameObject obj)
    {
        var unit = obj.GetComponent<Unit>();
        UnitData data = Game.Instance.GetUnitData(unit);

        if (nameInfo != null) nameInfo.text = data.Name;
        if (healthInfo != null) healthInfo.text = $"´{unit.CurrentHealth} / {data.HealthMax}";
        if (manaInfo != null) manaInfo.text = $"´{unit.CurrentMana} / {data.ManaMax}";

        if (icon != null)
        {
            icon.sprite = data.SelectionInfoIcon;
            icon.gameObject.SetActive(true);
        }


    }
   
    void UpdateInfoOnDeselect()
    {
        if (nameInfo != null) nameInfo.text = "";
        if (healthInfo != null) healthInfo.text = "";
        if (manaInfo != null) manaInfo.text = "";

        if (icon != null)
        {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
        }            
    }

    #endregion
}
