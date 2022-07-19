using System;
using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private string confirmationButtonName = "Fire1";
    [SerializeField] private string toggleInventoryButtonName = "Fire2";
    [SerializeField] private string axisHorizontal = "Horizontal";
    [SerializeField] private string axisVertical = "Vertical";

    [SerializeField] private float lastAxisX;
    [SerializeField] private float lastAxisY;


    private static InputManager _Instance;
    public static InputManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<InputManager>();
            }

            return _Instance;
        }
    }

    public bool IsPressingConfirmation()
    {
        return Input.GetButtonDown(confirmationButtonName);
    }
    
    public bool IsPressingToggleInventory()
    {
        return Input.GetButtonDown(toggleInventoryButtonName);
    }

    public (float x, float y) GetBothAxis()
    {
        var x = Input.GetAxisRaw(axisHorizontal);
        var y = Input.GetAxisRaw(axisVertical);

        lastAxisX = x > 0 ? 1f : x < 0 ? -1f : 0;
        lastAxisY = y > 0 ? 1f : y < 0 ? -1f : 0;

        return (lastAxisX, lastAxisY);
    }
}