using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [SerializeField] private ResourceManager resmgr;
    [SerializeField] private SelectionHandler selectionHandler;
    [SerializeField] private PlayerManager playerManager; 

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public int GetResAmount(ResourceType type)
    {
        return resmgr.GetResoureAmount(type);
    }

    public bool CheckResRequirement(int amount, ResourceType type)
    {
        return resmgr.CheckResourceRequirement(amount, type);
    }

    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public Unit GetSelectedObj()
    {
        return selectionHandler.ActiveUnit;
    }

    public void MousePositionCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Game.Instance.GetMousePosition());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 999f))
        {
                       
        }
    }

    public void AddPlayerUnit(Unit unit, PlayerString owner)
    {
        playerManager.AddUnit(unit, owner);
    }

    public void AddPlayerUnit(Unit unit, PlayerType type)
    {
        playerManager.AddUnit(unit, type);
    }

    public void RemovePlayerUnit(Unit unit, PlayerString owner)
    {
        playerManager.RemoveUnit(unit, owner);
    }

    public bool HumanContolledUnit(Unit unit)
    {
        return playerManager.HumanConrolledUnit(unit);
    }
}
