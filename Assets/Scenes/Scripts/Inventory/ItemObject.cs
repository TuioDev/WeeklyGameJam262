using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, Interactable
{
    private Interactable source;
    [SerializeField] private Dialog dialog;
    [SerializeField] private bool isPickable = true;

    public InventoryItemData inventoryItemData;
    public void Interact(Interactable source)
    {
        if (!isPickable) return;

        this.source = source;
        InventoryManager.Instance.Add(inventoryItemData);
        if(dialog?.Lines.Count > 0)
        {
            DialogManager.Instance.ShowDialogAndNotifyWhenClosed(dialog, this);
        }
    }

    public void IsDoneInteracting()
    {
        source?.IsDoneInteracting();
        source = null;

        Destroy(gameObject);
    }
}
