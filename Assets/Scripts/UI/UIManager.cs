using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject inventoryBox;
    [SerializeField] private GameObject itemsArea;

    private List<Transform> allIcons = new List<Transform>();
    private List<Transform> allStackNumbers = new List<Transform>();

    private Interactable showInventorySource;
    private int currentInventorySize;
    private int selectedItem = 0;
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
        UpdateInventoryList(allIcons);
        ToggleInventory();
        isOpen = true;
    }

    public void IsDoneInteracting()
    {
        isOpen = false;
        ToggleInventory();
        StartCoroutine(NotifySource());
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
