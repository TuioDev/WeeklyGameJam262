using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, IInteractable
{
	public int maximunNumberOfToDoLogs = 12;

    [SerializeField] private GameObject inventoryBox;
	private IInteractable showInventorySource;
	private bool isOpen;

	public GameObject ToDoItems;
	[SerializeField] private GameObject ToDoLogItemPrefab;

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
		if (InputManager.Instance.IsPressingToggleInventory())
		{
			if(!isOpen)
            {
				PlayerManager.Instance.PlayerLock(true);
				Interact(null);
            } 
			else
            {
				IsDoneInteracting();
				PlayerManager.Instance.PlayerLock(false);
			}
		}
    }
	
	public void ToggleInventory() => inventoryBox.SetActive(!inventoryBox.activeSelf);

	private IEnumerator NotifySource()
	{
		// The player can't interact with the Interactable in the same frame
		yield return new WaitForEndOfFrame();
		showInventorySource?.IsDoneInteracting();
		showInventorySource = null;
	}

    public void Interact(IInteractable source)
    {
		showInventorySource = source;
		OpenMenu();
    }

    public void IsDoneInteracting()
    {
		CloseMenu();
		StartCoroutine(NotifySource());
    }

	public void OpenMenu()
    {
		UpdateToDoList();
		ToggleInventory();
		isOpen = true;
	}

	public void CloseMenu()
    {
		isOpen = false;
		ToggleInventory();
	}

	public void UpdateToDoList()
    {
		int index = 0;
		foreach(ToDoItem toDoItem in ToDoManager.Instance.Items)
        {
			GameObject item = ToDoItems.transform.GetChild(index).gameObject;
			TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
			text.SetText(toDoItem.Text);
			text.fontStyle = toDoItem.IsDone ? FontStyles.Strikethrough : FontStyles.Normal;

			item.SetActive(true);

			index++;
        }

		for(; index < maximunNumberOfToDoLogs; index++)
        {
			ToDoItems.transform.GetChild(index).gameObject.SetActive(false);
		}
    }
}
