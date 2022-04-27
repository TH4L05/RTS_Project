using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public GameData gameData;

    [SerializeField] private UnitSelection selectionHandler;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ResourceInfo resourceInfo;
    [SerializeField] private BuildMode buildMode;
    [SerializeField] private CameraRig cameraRig;
    [SerializeField] private IngameUI ingameUI;

    public UnitSelection SelectionHandler => selectionHandler;
    public PlayerManager PlayerManager => playerManager;
    public ResourceInfo ResourceInfo => resourceInfo;
    public BuildMode BuildMode => buildMode;
    public CameraRig CameraRig => cameraRig;
    public IngameUI IngameUI => ingameUI;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }
}
