using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Undefined = -1,
    Worker,
    Melee,
    Range
}

public enum MovementType
{
    Undefined = -1,
    Normal,
    Fly,
}

[CreateAssetMenu(fileName = "NewCharData", menuName = "Data/CharacterData")]
public class CharacterData : UnitData
{
    #region SerializedFields

    [Header("Character")]
    [SerializeField] private CharacterType charType = CharacterType.Undefined;
    [SerializeField] private MovementType movementType = MovementType.Undefined;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float movementAccerlation = 1f;

    #endregion

    #region PublicFields

    public CharacterType CharType => charType;
    public MovementType MovementType => movementType;
    public float MovementSpeed => movementSpeed;
    public float MovementAccerlation => movementAccerlation;

    #endregion


    public override void SetDataFromStrings(string[] data)
    {
        base.SetDataFromStrings(data);

        charType = Enum.Parse<CharacterType>(data[37]);
        movementType = Enum.Parse<MovementType>(data[38]);
        movementSpeed = float.Parse(data[39]);
        movementAccerlation = float.Parse(data[40]);

    }

}
