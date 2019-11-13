using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SlotScript : MonoBehaviour, IDropHandler{

	public int slotID;
	public Inventory inv;


	void Start(){
		inv = FindObjectOfType<Inventory> ();
	}

	public void OnDrop (PointerEventData eventData)
	{
		GameObject dropedItem = eventData.pointerDrag;
	
		if (inv.itemCollection [slotID] == null) {

			dropedItem.transform.SetParent (transform);
			dropedItem.transform.position = transform.position;
			dropedItem.GetComponent<ItemScript> ().originalParent = dropedItem.transform.parent;
			inv.itemCollection [dropedItem.GetComponent<ItemScript> ().currentSlotID] = null;
			dropedItem.GetComponent<ItemScript> ().currentSlotID = slotID;
			inv.itemCollection [slotID] = dropedItem.GetComponent<Item> ();
		} else {
			
			Transform thisItem = transform.GetChild (0);

			Item temp = thisItem.GetComponent<Item> ();
			inv.itemCollection [slotID] = inv.itemCollection[dropedItem.GetComponent<ItemScript>().currentSlotID];
			inv.itemCollection [dropedItem.GetComponent<ItemScript> ().currentSlotID] = temp;

			thisItem.GetComponent<ItemScript> ().currentSlotID = dropedItem.GetComponent<ItemScript> ().currentSlotID;
			dropedItem.GetComponent<ItemScript> ().currentSlotID = slotID;

			thisItem.SetParent (dropedItem.GetComponent<ItemScript> ().originalParent);
			dropedItem.transform.SetParent (transform);

			dropedItem.GetComponent<ItemScript> ().originalParent = transform;
			thisItem.GetComponent<ItemScript> ().originalParent = thisItem.transform.parent;

			thisItem.position = thisItem.parent.position;
			dropedItem.transform.position = transform.position;
		}
	}

	//todo zrobic zeby wracal jak go wypierdole poza okno
}
