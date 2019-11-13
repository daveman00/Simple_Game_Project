using UnityEngine;
//using System.Collections;

public class CameraFollow : MonoBehaviour {
	//obiekt do sledzenia
	public GameObject target;
	private Vector3 targetPosition;

	public float followSpeed;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		targetPosition = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, targetPosition, followSpeed * Time.deltaTime);
	}
}