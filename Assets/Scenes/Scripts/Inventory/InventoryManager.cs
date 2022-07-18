using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Dictionary<InventoryItemData, InventoryItem> ItemDictionary;
    public List<InventoryItem> Inventory { get; private set; }

	private static InventoryManager _Instance;
	public static InventoryManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = FindObjectOfType<InventoryManager>();
			}

			return _Instance;
		}
	}

	void Awake()
	{
		Inventory = new List<InventoryItem>();
		ItemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
	}

	public void Add(InventoryItemData inventoryItemData)
	{
		if (ItemDictionary.TryGetValue(inventoryItemData, out InventoryItem value))
		{
			value.AddToStack();
		}
		else
		{
			InventoryItem newItem = new InventoryItem(inventoryItemData);
			Inventory.Add(newItem);
			ItemDictionary.Add(inventoryItemData, newItem);
		}
	}
	public void Remove(InventoryItemData inventoryItemData)
	{
		if (ItemDictionary.TryGetValue(inventoryItemData, out InventoryItem value))
		{
			value.RemoveFromStack();

			if (value.StackSize == 0)
			{
				Inventory.Remove(value);
				ItemDictionary.Remove(inventoryItemData);
			}
		}
	}
}
