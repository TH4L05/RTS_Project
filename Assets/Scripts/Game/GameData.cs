/// <author> Thomas Krahl </author>

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public List<ResourceSlot> resources = new List<ResourceSlot>();

    public List<ResourceSetup> reqs = new List<ResourceSetup>();

    public List<GameObject> buildingList = new List<GameObject>();
    public List<GameObject> characterList = new List<GameObject>();
}
