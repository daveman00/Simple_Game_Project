using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public int id;
	public Sprite Sprite;
	public string name;

	public Item cloneItem(Item itemToClone){
		id = itemToClone.id;
		Sprite = itemToClone.Sprite;
		name = itemToClone.name;
		return this;
	}
		
}
