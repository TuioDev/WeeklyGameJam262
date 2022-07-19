using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog dialog;
    private Interactable source;

    public void Interact(Interactable source)
    {
        this.source = source;
        DialogManager.Instance.ShowDialogAndNotifyWhenClosed(dialog, this);
    }

    public void IsDoneInteracting()
    {
        source?.IsDoneInteracting();
        source = null;
    }
}
