using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionHandler : MonoBehaviour
{
    public static Action<Unit> ObjectSelected;
    public static Action ObjectDeselected;

    private Camera cam;
    public List<Unit> selectedUnits = new List<Unit>();
    private int selectedIndex;
    private RaycastHit hit;

    public Unit ActiveUnit => selectedUnits[0];
    

    private void Awake()
    {
        cam = Camera.main;
        Unit.UnitIsDead += DeselectIfUnitIsDead;
    }

    private void OnDestroy()
    {
        Unit.UnitIsDead -= DeselectIfUnitIsDead;
    }

    void Update()
    {
        MousePosRayCast();
        //ShowHealthbar();
        CheckLeftMouseClick();
        CheckRightMouseClick();
    }

    private void MousePosRayCast()
    {
        Ray ray = cam.ScreenPointToRay(Game.Instance.GetMousePosition());
        Physics.Raycast(ray, out hit, 999f);
    }

    /*private void ShowHealthbar()
    {
        var selectable = hit.transform.GetComponent<Unit>();
        if (!selectable) return;
        Debug.Log("ShowHealthBar");
    }*/

    void CheckLeftMouseClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !BuildMode.IsActive)
        {
            var selectable = hit.transform.GetComponent<Unit>();
            
            if (selectable)
            {
                DeselectUnit();
                SelectUnit(selectable);
            }
            else if(selectedUnits.Count > 0)
            {
              if (selectedUnits[0].UnitType == UnitType.Character && Game.Instance.HumanContolledUnit(selectedUnits[0]))
                {
                    var unit = selectedUnits[0].GetComponent<Character>();
                    unit.SetTarget(hit);
                }
            }                                      
        }
    }

    #region Select&Deselect

    public void SelectUnit(Unit unit)
    {
        selectedUnits.Add(unit);
        SelectionCircleVisibility(true);
        ObjectSelected?.Invoke(selectedUnits[0]);
    }

    public void DeselectUnit()
    {
        SelectionCircleVisibility(false);
        selectedUnits.Clear();
        ObjectDeselected?.Invoke();
    }

    private void DeselectIfUnitIsDead(GameObject obj)
    {
        if (selectedUnits[0] == obj)
        {
            DeselectUnit();
        }
    }

    #endregion


    private void CheckRightMouseClick()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && !BuildMode.IsActive)
        {
            if (selectedUnits.Count > 0)
            {
                DeselectUnit();
            }
        }
    }

    private void SelectionCircleVisibility(bool visible)
    {
        if (selectedUnits.Count == 0) return;

        foreach (var unit in selectedUnits)
        {             
            unit.ChangeSelectionCircleVisibility(visible);         
        }
    }


    

    /*public GameObject GetSelectedObject(string name)
    {
        foreach (var obj in selectedUnits)
        {
            if (obj.GetComponent<UnitData>().Name == name)
            {
                return obj;
            }
        }

        return null;
    }*/
}
