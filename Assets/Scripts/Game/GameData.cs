using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public List<GameObject> buildingList = new List<GameObject>();
    public List<GameObject> characterList = new List<GameObject>();
}
