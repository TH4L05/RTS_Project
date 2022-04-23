using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public GameData gameData;

    //[SerializeField] private ResourceManager resourceManager;
    [SerializeField] private SelectionHandler selectionHandler;
    [SerializeField] private PlayerManager playerManager; 
    [SerializeField] private ResourceInfo resourceInfo;
    [SerializeField] private ActionsGrid buildButtonGrid;
    [SerializeField] private BuildMode buildMode;

    //public ResourceManager ResourceManager => resourceManager;
    public SelectionHandler SelectionHandler => selectionHandler;
    public PlayerManager PlayerManager => playerManager;
    public ResourceInfo ResourceInfo => resourceInfo;
    public BuildMode BuildMode => buildMode;

    public Tooltip tooltipUI;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void Setup()
    {
        int  index = 0;
        foreach (var building in gameData.buildingList)
        {
            buildButtonGrid.actionButtons[index].SetAction(gameData.buildingList[index].GetComponent<Unit>(),0);
        }
    }

    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
}
