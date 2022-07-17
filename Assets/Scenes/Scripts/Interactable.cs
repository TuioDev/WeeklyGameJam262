using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public void Interact(Interactable source);
    public void IsDoneInteracting();
}
