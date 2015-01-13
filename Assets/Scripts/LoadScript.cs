using UnityEngine;
using System.Collections;

public class LoadScript : MonoBehaviour {

	void Update() {
		if (Input.GetMouseButton(0)) {
			Application.LoadLevel("game");
		}
	}

	void OnGUI() {
		if (Event.current.type == EventType.KeyDown) {
			Application.LoadLevel("game");
		}
	}
}
