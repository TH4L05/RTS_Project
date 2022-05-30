/// <author> Thomas Krahl </author>

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
