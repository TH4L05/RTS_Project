using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildQueueBar : MonoBehaviour
{
    [SerializeField] private List<QueueButton> queueButtons = new List<QueueButton>();
    private GameObject selectedObject;


    private void Start()
    {
        UnitSelection.ObjectSelected += OnSelection;
        UnitSelection.ObjectDeselected += OnDeselection;
        Building.UpdateFill += UpdateButtons;
    }

    private void OnDestroy()
    {
        UnitSelection.ObjectSelected -= OnSelection;
        UnitSelection.ObjectDeselected -= OnDeselection;
        Building.UpdateFill -= UpdateButtons;
    }

    private void OnSelection(GameObject obj)
    {
        var unit = obj.GetComponent<Unit>();
        if (unit.UnitData.Type != UnitType.Building) return;
        selectedObject = obj;
        /*var building = obj.GetComponent<Building>();
        UpdateQueue(building.buildCount);*/
    }

    private void OnDeselection()
    {
        selectedObject = null;
        foreach (var button in queueButtons)
        {
            button.HideButton();
        }
    }

    private void UpdateButtons(GameObject obj, List<BuildJob> jobs)
    {
        if (obj != selectedObject) return;

        foreach (var item in queueButtons)
        {
            item.HideButton();
        }

        var index = 0;
        foreach (var item in jobs)
        {
            queueButtons[index].ShowButton();
            queueButtons[index].SetIcon(item.sprite);
            queueButtons[index].SetFillAmount(item.fillamount);
                       
            index++;
        }
    }

    /*private void UpdateQueue(int count)
    {
        int index = 0;
        foreach (var button in queueButtons)
        {
            button.ChangeVisibility(false);          
            index++;
        }

        for (int i = 0; i < count; i++)
        {
            queueButtons[i].ChangeVisibility(true);
        }
    }*/
}
