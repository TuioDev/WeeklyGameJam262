using UnityEngine;

public class NPCWithDialog : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialog dialog;
    private IInteractable source;

    public void Interact(IInteractable source)
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
