
using System;

public interface IInteractable
{
    public void Interact(IInteractable source);
    public void IsDoneInteracting();
}
