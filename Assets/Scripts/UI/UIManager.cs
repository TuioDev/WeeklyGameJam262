using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject inventoryBox;
	private Interactable showInventorySource;
	private bool isOpen;

	private static UIManager _Instance;
	public static UIManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = FindObjectOfType<UIManager>();
			}

			return _Instance;
		}
	}

	void Update()
    {
		DoInteraction();
    }

	public void DoInteraction()
    {
        if (InputManager.Instance.IsPressingToggleInventory() && isOpen)
        {
			IsDoneInteracting();
        }
    }
	public void ToggleInventory()
    {
		inventoryBox.SetActive(!inventoryBox.activeSelf);
    }

	private IEnumerator NotifySource()
	{
		// The player can't interact with the Interactable in the same frame
		yield return new WaitForEndOfFrame();
		showInventorySource?.IsDoneInteracting();
		showInventorySource = null;
	}

    public void Interact(Interactable source)
    {
		showInventorySource = source;
		ToggleInventory();
		isOpen = true;
    }

    public void IsDoneInteracting()
    {
		isOpen = false;
		ToggleInventory();
		StartCoroutine(NotifySource());
    }
}
