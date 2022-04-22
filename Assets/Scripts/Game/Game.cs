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

    //public ResourceManager ResourceManager => resourceManager;
    public SelectionHandler SelectionHandler => selectionHandler;
    public PlayerManager PlayerManager => playerManager;
    public ResourceInfo ResourceInfo => resourceInfo;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
}
