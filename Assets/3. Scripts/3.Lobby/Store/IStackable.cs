using TMPro;

public interface IStackable
{
    TextMeshProUGUI GetStackText
    {
        get;
    }

    int GetCount
    {
        get;
    }
}
