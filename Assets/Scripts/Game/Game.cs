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
}
