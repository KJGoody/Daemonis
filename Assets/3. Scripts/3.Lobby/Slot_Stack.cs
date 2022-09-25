using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public abstract class Slot_Stack : Slot_Base, IStackable
{
    protected ObservableStack<Item_Base> Items = new ObservableStack<Item_Base>();
    public int GetCount { get { return Items.Count; } }
    public bool IsEmpty { get { return Items.Count == 0; } }
    public ObservableStack<Item_Base> GetItems { get { return Items; } }
    public Item_Base Item
    {
        get
        {
            if (!IsEmpty)
                return Items.Peek();

            return null;
        }
    }

    [SerializeField] protected TextMeshProUGUI StackSize;
    public TextMeshProUGUI GetStackText { get { return StackSize; } }

    protected virtual void Awake()
    {
        Items.OnPop += new UpdateStackEvent(UpdateSlot);
        Items.OnPush += new UpdateStackEvent(UpdateSlot);
        Items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public abstract bool AddItem(Item_Base item);
    public abstract bool RemoveItem();
    public abstract bool StackItem(Item_Consumable item);
}
