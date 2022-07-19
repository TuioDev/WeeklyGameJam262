using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IInteractable
{
	public int maximunNumberOfToDoLogs = 12;

    [SerializeField] private GameObject inventoryBox;
    [SerializeField] private GameObject itemsArea;

    private List<Transform> allIcons = new List<Transform>();
    private List<Transform> allStackNumbers = new List<Transform>();

    private Interactable showInventorySource;
    private int currentInventorySize;
    private int selectedItem = 0;
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

    void Awake()
    {
        SetUpIconList();
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
        UpdateInventoryList(allIcons);
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

    public void SetUpIconList()
    {
        foreach (Transform child in itemsArea.transform)
        {
            var area = child.gameObject.GetComponentInChildren<Image>().transform;

            allIcons.Add(area.Find("Icon").transform);
        }
    }
    public void UpdateInventoryList(List<Transform> iconsList)
    {
        foreach (Transform icon in iconsList)
        {
            icon.gameObject.GetComponent<Image>().sprite = null;
        }

        currentInventorySize = InventoryManager.Instance.Inventory.Count;

        if (currentInventorySize == 0) return;

        for (int i = 0; i < currentInventorySize; i++)
        {
            InventoryItem item = InventoryManager.Instance.Inventory[i];
            iconsList[i].GetComponent<Image>().sprite = item.Data.SpriteInventory;
        }
    }
}
