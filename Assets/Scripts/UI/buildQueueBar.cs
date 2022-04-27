using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildQueueBar : MonoBehaviour
{
    [SerializeField] private List<QueueButton> queueButtons = new List<QueueButton>();

    private void Start()
    {
        SelectionHandler.ObjectSelected += OnSelection;
        SelectionHandler.ObjectDeselected += OnDeselection;
    }

    private void OnDestroy()
    {
        SelectionHandler.ObjectSelected -= OnSelection;
        SelectionHandler.ObjectDeselected -= OnDeselection;
    }


    private void OnSelection(GameObject obj)
    {
        var unit = obj.GetComponent<Unit>();
        if (unit.UnitData.Type != UnitType.Building) return;
        var building = obj.GetComponent<Building>();
        UpdateQueue(building.buildCount);
    }

    private void OnDeselection()
    {
        foreach (var button in queueButtons)
        {
            button.ChangeVisibility(false);
        }
    }

    private void UpdateQueue(int count)
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
    }
}
