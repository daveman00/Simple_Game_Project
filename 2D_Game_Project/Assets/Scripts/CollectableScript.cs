using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {
	

	private Inventory inventory;
	private Item item;


	void Start(){
		inventory = FindObjectOfType<Inventory> ();
		item = GetComponent<Item> ();
	}

	public void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player" && !inventory.Full ()) {
			inventory.AddNewItem (item);
		}
	}

}
