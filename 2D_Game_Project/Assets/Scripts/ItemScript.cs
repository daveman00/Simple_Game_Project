using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
	public int currentSlotID;
	public Transform originalParent;

	void Start(){
		originalParent = transform.parent;
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (gameObject != null) {
			gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			transform.position = eventData.position;
			transform.SetParent (transform.parent.parent);
		}
	}
		
	public void OnDrag (PointerEventData eventData)
	{
		if (gameObject != null) {
			transform.position = eventData.position;
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		gameObject.transform.name = gameObject.GetComponent<Item> ().name;
		gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}
}
