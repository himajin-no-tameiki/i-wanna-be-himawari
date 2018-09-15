using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	[HideInInspector]
	public static LevelManager instance = null;
	[HideInInspector]
	public GameObject playerObject = null;
	[HideInInspector]
	public Vector3 savedPosition = new Vector3(-32, 13, 0);

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	void Start () {
	}

	void Update () {
		if (Input.GetButtonDown("Reset")) {
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		}
	}

	public void SaveGame() {
		Debug.Log("Save!!");
		if (playerObject)
			savedPosition = playerObject.transform.position;
	}
}
