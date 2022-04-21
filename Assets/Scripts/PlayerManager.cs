using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();


    private void Awake()
    {
        foreach (var player in players)
        {
            player.Setup();
        }
    }

    public void AddUnit(Unit unit, PlayerString owner)
    {
        foreach (var player in players)
        {
            if (player.name == owner)
            {
                player.AddUnit(unit);
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
            }
        }
    }

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

    public PlayerString GetPlayerStringFromPlayer(PlayerType type)
    {
        foreach (var player in players)
        {
            if (player.type == type)
            {
                player.GetPlayerString();
            }
        }
        return PlayerString.Undefined;
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
