using UnityEngine;

public class NPCThatGivesTasks : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialog dialogFirstInteraction;
    [SerializeField] private Dialog dialogSecondInteraction;
    private IInteractable source;

    [SerializeField] private bool isFirstInteractionDone;

    public void Interact(IInteractable source)
    {
        this.source = source;
        if(!isFirstInteractionDone)
        {
            FirstInteraction();
        }
        else
        {
            SecondInteraction();
        }
    }

    public void IsDoneInteracting()
    {
        source?.IsDoneInteracting();
        source = null;
    }

    public void FirstInteraction()
    {
        ToDoManager.Instance.AddToDoItem("task1", "essa é uma task");
        ToDoManager.Instance.AddToDoItem("task2", "essa é outra task");
        DialogManager.Instance.ShowDialogAndNotifyWhenClosed(dialogFirstInteraction, this);

        isFirstInteractionDone = true;
    }

    public void SecondInteraction()
    {
        ToDoManager.Instance.FinishToDoItem("task1");
        ToDoManager.Instance.FinishToDoItem("task2");
        DialogManager.Instance.ShowDialogAndNotifyWhenClosed(dialogSecondInteraction, this);
    }
}
