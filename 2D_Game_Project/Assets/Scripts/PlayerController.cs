using UnityEngine;
//using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float restartLevelDelay = 1f;
	//predkosc poruszania sie
	public float moveSpeed;

	//flaga do ruchu w osi x
	private bool horizontal;
	//flaga do ruchu w osi y
	private bool vertical;

	private Rigidbody2D rb2d;
	private Animator anim;

	//flaga czy gracz sie porusza
	private bool isMoving;

	//local scale on start
	Vector3 localScale;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		localScale = transform.localScale;
	}

	// Update is called once per frame
	void Update () {
		isMoving = false;

		if (Input.GetAxisRaw ("Horizontal") != 0 && (!vertical)) {
			rb2d.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal") * moveSpeed, 0f);
			horizontal = true;
			isMoving = horizontal;
		}


		if (Input.GetAxisRaw ("Vertical") != 0 && (!horizontal)) {
			rb2d.velocity = new Vector2 (0f, Input.GetAxisRaw ("Vertical") * moveSpeed);
			vertical = true;
			isMoving = vertical;
		}


		if (Input.GetAxisRaw ("Horizontal") == 0) {
			rb2d.velocity = new Vector2 (0f, Input.GetAxisRaw ("Vertical") * moveSpeed);
			horizontal = false;
		}


		if (Input.GetAxisRaw ("Vertical") == 0) {
			rb2d.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal") * moveSpeed, 0f);
			vertical = false;
		}

		flipX ();
		anim.SetBool ("moving", isMoving);

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Exit")
		{
			Invoke("Restart", restartLevelDelay);
			//enabled = false;
		}
		else if (other.tag == "Treasure")
		{
			other.gameObject.SetActive(false);
		}
	}

	private void Restart()
	{	
		//DestroyBoard();
		SceneManager.LoadScene("Scene");
	}

	//obraca teksturke po osi x
	public void flipX(){
		if (Input.GetAxisRaw ("Horizontal") < 0 && !vertical) {
			transform.localScale = new Vector3 (-localScale.x, localScale.y, localScale.z);
		}
		else if (Input.GetAxisRaw ("Horizontal") > 0 && !vertical){
			transform.localScale = new Vector3 (localScale.x, localScale.y, localScale.z);
		}
		
	}

	
}