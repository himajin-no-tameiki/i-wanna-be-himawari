﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HimaController : MonoBehaviour {

	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]
	public bool jump = false;

	public UnityEvent deathEvent;

	public float moveAccel = 30f;
	public float maxSpeed = 2f;
	public float jumpSpeed = 10f;
	public int maxAirJumps = 2;
	public float distanceToGround = 0.78f;
	public GameObject deathParticle;

	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	private int jumpCount;
	private int groundLayerMask;

	// Use this for initialization
	void Awake() {
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
	}

	void Start() {
		LevelManager.instance.playerObject = gameObject;
		transform.position = LevelManager.instance.savedPosition;

		if (deathEvent == null)
			deathEvent = new UnityEvent();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Jump") && jumpCount < maxAirJumps) {
			jump = true;
			jumpCount += 1;
		}
	}

	bool isGrounded() {
//		return rb2d.IsTouchingLayers(groundLayer);

		Vector2 position = transform.position;
		Vector2 direction = Vector2.down;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distanceToGround, groundLayerMask);
		return hit.collider != null;
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

		anim.SetFloat("VelocityY", rb2d.velocity.y);

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();

		bool nextGrounded = isGrounded();


		if (jump) {
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
			jump = false;
		}

		if (nextGrounded) {
			jumpCount = 0;
		}

		if (nextGrounded && !grounded) {

			anim.SetTrigger("Land");
		}

		if (!nextGrounded && grounded) {
			anim.SetTrigger("Midair");
		}

		grounded = nextGrounded;

		if (isInsideGround()) {
			Die();
		}
	}

	bool isInsideGround() {
		return Physics2D.OverlapPoint(transform.position, groundLayerMask);
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

		deathEvent.Invoke();
		LevelManager.instance.OnDeath();
	}
}
