using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveButton : MonoBehaviour, IDamageable {

//	public UnityEvent saveEvent;

	// Use this for initialization
	void Start () {
//		if (saveEvent == null) 
//			saveEvent = new UnityEvent();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(float damage) {
		save();
	}

	void save() {
//		saveEvent.Invoke();
		LevelManager.instance.SaveGame();
	}
}
