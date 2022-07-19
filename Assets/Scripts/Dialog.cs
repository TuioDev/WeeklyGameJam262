using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] private List<string> lines;

    public List<string> Lines
    {
        get { return lines; }
    }

    // We can set the NPC to give an item to the player
}
