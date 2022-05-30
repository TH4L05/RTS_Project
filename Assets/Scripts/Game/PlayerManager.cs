/// <author> Thomas Krahl </author>

using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public int myPlayerIndex = -1;
    public GameObject unitsObject;

    private void Awake()
    {
        int index = 0;
        foreach (var player in players)
        {
            var obj = Instantiate(new GameObject(player.name.ToString()));
            obj.transform.parent = transform;

            if (player.type == PlayerType.Human)
            {
                myPlayerIndex = index;
            }
            index++;
        }
    }

    private void Start()
    {
        int index = 0;

        foreach (var player in players)
        {
            var data = Game.Instance.gameData;
            player.Setup(data);

            if (player.type == PlayerType.Human)
            {
                myPlayerIndex = index;
            }

            index++;
        }
    }

    #region AddUnit

    public void AddUnit(Unit unit, PlayerString owner)
    {
        foreach (var player in players)
        {
            if (player.name == owner)
            {
                player.AddUnit(unit);
                unit.transform.parent = unitsObject.transform;
            }
        }
    }

    public void AddUnit(Unit unit, PlayerType type)
    {
        foreach (var player in players)
        {
            if (player.type == type)
            {
                player.AddUnit(unit);
                unit.transform.parent = unitsObject.transform;
            }
        }
    }

    #endregion

    #region RemoveUnit

    public void RemoveUnit(Unit unit, PlayerString owner)
    {
        foreach (var player in players)
        {
            if (player.name == owner)
            {
                player.RemoveUnit(unit);
            }
        }
    }

    public void RemoveUnit(Unit unit, PlayerType type)
    {
        foreach (var player in players)
        {
            if (player.type == type)
            {
                player.RemoveUnit(unit);
            }
        }
    }

    #endregion

    public bool CheckResourceRequirement(PlayerString player, int requiredAmount, ResourceType resourceType)
    {
        foreach (var playr in players)
        {
            if (playr.name != player) continue;
            return playr.resourceManager.CheckResourceRequirement(requiredAmount, resourceType);
        }
        return false;
    }

    public PlayerString GetPlayerStringFromPlayer(PlayerType type)
    {
        return players[myPlayerIndex].GetPlayerString();
    }

    public bool HumanConrolledUnit(Unit unit)
    {
        foreach (var player in players)
        {
            if (player.type == PlayerType.Human)
            {
                Debug.Log(player.name);
                return player.PlayerOwnsUnit(unit);
            }
        }
        return false;
    }

}
