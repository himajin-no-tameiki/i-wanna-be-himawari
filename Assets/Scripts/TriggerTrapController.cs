using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrapController : MonoBehaviour {

	public Vector3 TriggeredVelocity;

	private bool triggered = false;

	void Start () {
		
	}

	void FixedUpdate () {
		if (triggered) {
			transform.position += TriggeredVelocity * Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			triggered = true;

			if (!gameObject.CompareTag("Trap")) {
				gameObject.tag = "Trap";
			}
		}
	}

	void OnBecameInvisible() {
		Destroy(gameObject, 0.5f);
	}
}
