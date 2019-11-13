using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour{
	private const int SLOTS_AMOUNT = 18;
	public Item[] itemCollection;
	//public List<Item> itemCollection = new List<Item>();
	public List<GameObject> slots = new List<GameObject> ();


	GameObject inventoryPanel;
	GameObject slotPanel;

	public GameObject slotPrefab;
	public GameObject itemPrefab;



	void Start(){
		
		inventoryPanel = GameObject.Find ("Inventory Panel");
		slotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject;

		itemCollection = new Item[SLOTS_AMOUNT];
		for (int i = 0; i < SLOTS_AMOUNT; i++) {
		
			slots.Add(Instantiate (slotPrefab));
			slots [i].GetComponent<SlotScript> ().slotID = i;
			slots [i].transform.SetParent (slotPanel.transform);
		}
	}
		
	public void AddNewItem(Item itemToAdd){
		for (int i = 0; i < SLOTS_AMOUNT; i++) {

			if (itemCollection [i] == null) {
				itemCollection [i] = itemToAdd;

				GameObject itemReference = Instantiate (itemPrefab);
				itemReference.transform.name = itemToAdd.name;
				itemReference.GetComponent<Item> ().cloneItem (itemToAdd);
				itemReference.transform.SetParent (slots [i].transform);
				itemReference.GetComponent<Image> ().sprite = itemToAdd.Sprite;
				itemReference.transform.position = itemReference.transform.parent.position;
				itemReference.GetComponent<ItemScript> ().currentSlotID = i;
				break;
			}
		}
	}

	//zwraca czy mamy pelne eq czy nie
	public bool Full(){
		for (int i = 0; i < SLOTS_AMOUNT; i++) {
			if (itemCollection [i] == null)
				return false;
		}
		return true;
	}
}
