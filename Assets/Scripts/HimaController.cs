using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HimaController : MonoBehaviour {

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveAccel = 30f;
	public float maxSpeed = 2f;
	public float jumpSpeed = 10f;
	public int maxAirJumps = 2;
	public GameObject deathParticle;

	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	private int jumpCount;

	// Use this for initialization
	void Awake() {
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Jump") && jumpCount < maxAirJumps) {
			jump = true;
			jumpCount += 1;
			Debug.Log("jump");
		}
	}

	bool isGrounded() {
		int groundLayer = 1 << LayerMask.NameToLayer("Ground");
		return rb2d.IsTouchingLayers(groundLayer);
	}

	void FixedUpdate() {
		float h = Input.GetAxis("Horizontal");

		if (h == 0) {
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);
			anim.SetBool("Walking", false);
		} else {
			float vx = Mathf.Sign(h) * maxSpeed;
			rb2d.velocity = new Vector2(vx, rb2d.velocity.y);
			anim.SetBool("Walking", true);
		}


//		anim.SetFloat("Speed", Mathf.Abs(h));
		anim.SetFloat("VelocityY", rb2d.velocity.y);

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();

//		if (jump) {
//			anim.SetTrigger("Jump");
//			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
//			jump = false;
//		}
//
		bool nextGrounded = isGrounded();

//		if (grounded && !jump) {
//			jumpCount = 0;
//			anim.SetTrigger("Land");
//			Debug.Log("land");
//		}

		if (jump) {
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
			jump = false;
		}

		if (nextGrounded) {
			jumpCount = 0;
		}

		if (nextGrounded && !grounded) {

			anim.SetTrigger("Land");
			Debug.Log("land");
		}

		if (!nextGrounded && grounded) {
			anim.SetTrigger("Midair");
			Debug.Log("Midair");
		}

		grounded = nextGrounded;
	}


	void Flip() {
		facingRight = !facingRight;
		transform.localRotation = Quaternion.Euler(0f, facingRight ? 0f : 180f, 0f);
	}

	void OnCollisionEnter2D(Collision2D col) {

		if (col.gameObject.CompareTag("Trap")) {
			Die();
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log(LayerMask.LayerToName(col.gameObject.layer));
	}

	void Die() {
		Destroy(gameObject);
		GameObject.Instantiate(deathParticle, transform.position, Quaternion.identity);
	}
}
