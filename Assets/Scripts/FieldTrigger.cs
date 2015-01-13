using UnityEngine;
using System.Collections;

public class FieldTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D obj) {
		if (obj.name == "squareball") {
			GameObject.Find("Score").GetComponent<GameController>().canGoal = true;
		}
	}

}
