using UnityEngine;
using System.Collections;

public class GateTrigger : MonoBehaviour {
	
	void OnCollisionEnter2D(Collision2D collision) {
		if (Network.isServer) {
			if (collision.gameObject.name == "squareball") {
				GameObject.Find("Score").GetComponent<GameController>().onGoal(this.transform.position.x);
			}
		}
	}
}
