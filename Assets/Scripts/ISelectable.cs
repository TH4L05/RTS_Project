using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionType
{
    Undefined = -1,
    Unit,
    Destructable,
    Item
}

public interface ISelectable
{
   public void OnSelect();
   public void OnDeSelect();
}
