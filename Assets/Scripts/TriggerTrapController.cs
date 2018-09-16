using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrapController : MonoBehaviour {

	public Vector3 targetPositionRelative;
	public float rotateAngle;
	public float duration = 1f;

	private bool triggered = false;
	private float startTime;
	private Vector3 startPosition;
	private Quaternion startRotation;

	void Start () {
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

	void FixedUpdate () {
		if (triggered) {
			float currentTime = Mathf.Min(Time.time - startTime, duration);
			transform.position = startPosition + targetPositionRelative * currentTime / duration;

			float angle = rotateAngle * currentTime / duration;
			transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle)) * startRotation;

		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			triggered = true;
			startTime = Time.time;

			if (!gameObject.CompareTag("Trap")) {
				gameObject.tag = "Trap";
			}
		}
	}

	void OnBecameInvisible() {
		Destroy(gameObject, 0.5f);
	}
}
