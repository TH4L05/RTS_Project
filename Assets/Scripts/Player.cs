using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Human,
    Computer,
}

public enum PlayerString
{
    Undefined = -1,
    Player1,
    Player2,
    Player3,
    Player4,
    Player5,
    Player6,
    Player7,
    Player8,
}

[System.Serializable]
public class Player
{
    public PlayerString name;
    public PlayerType type;
    public Color playerColor;

    public List<Unit> ownedUnits = new List<Unit>();

    public void Setup()
    {
        foreach (var unit in ownedUnits)
        {
            bool isHumanControlled = false;
            if (type == PlayerType.Human)
            {
                isHumanControlled = true;
            }

            unit.SetOwner(name, isHumanControlled);
            unit.SetPlayerColor(playerColor);
        }
    }

    public PlayerString GetPlayerString()
    {
        return name;
    }

    public void AddUnit(Unit unit)
    {
        bool isHumanControlled = false;
        if (type == PlayerType.Human)
        {
            isHumanControlled = true;
        }

        unit.SetOwner(name, isHumanControlled);
        unit.SetPlayerColor(playerColor);
        ownedUnits.Add(unit);        
    }

    public void RemoveUnit(Unit unit)
    {
        ownedUnits.Remove(unit);
    }

    public bool PlayerOwnsUnit(Unit unit)
    {
        foreach (var ownedUnit in ownedUnits)
        {
            if (ownedUnit == unit)
            {
                Debug.Log(ownedUnit.name);
                return true;
            }
        }
        return false;
    }
}
