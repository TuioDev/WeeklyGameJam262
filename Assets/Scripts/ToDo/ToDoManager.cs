using System.Collections.Generic;
using UnityEngine;

public class ToDoManager : MonoBehaviour
{
	public List<ToDoItem> Items { get; private set; }

	private static ToDoManager _Instance;
	public static ToDoManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = FindObjectOfType<ToDoManager>();
			}

			return _Instance;
		}
	}

    private void Awake()
    {
		Items = new List<ToDoItem>();
	}

	public void AddToDoItem(string identifier, string text) => Items.Add(new ToDoItem(identifier, text));
	public void FinishToDoItem(string identifier) => Items.Find(toDo => toDo.Identifier == identifier)?.Done();
}