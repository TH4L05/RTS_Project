using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public GameData gameData;

    [SerializeField] private SelectionHandler selectionHandler;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ResourceInfo resourceInfo;
    [SerializeField] private BuildMode buildMode;
    [SerializeField] private Tooltip tooltipUI;

    public SelectionHandler SelectionHandler => selectionHandler;
    public PlayerManager PlayerManager => playerManager;
    public ResourceInfo ResourceInfo => resourceInfo;
    public BuildMode BuildMode => buildMode;
    public Tooltip TooltipUI => tooltipUI;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public UnitData GetUnitData(Unit unit)
    {
        UnitType type = unit.UnitType;

        switch (type)
        {
            case UnitType.Building:
                return unit.GetComponent<Building>().Data;

            case UnitType.Character:
                return unit.GetComponent<Character>().Data;

            default:
                break;
        }
        return null;
    }
}
