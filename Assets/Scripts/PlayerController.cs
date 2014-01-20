using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	const float SPEED_LIMIT = 0.1f;

	public bool isMoving = false;

	public Vector3 movingVector;	
	public int jumps = 1;

	void Start() {
		this.movingVector = new Vector3(0, 0, 0);
	}

	void Update() {

		if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0) {  // is pressed
			if (Mathf.Abs(this.movingVector.x) < SPEED_LIMIT) {    // before limit
				this.movingVector.x += (Input.GetAxisRaw("Horizontal") / 100);
			}
		} else {
			this.movingVector.x -= (this.movingVector.x / 10);  // inertion stops
			if (Mathf.Abs(this.movingVector.x) < 0.01f) {
				this.movingVector.x = 0;
			}
		}

		if (Input.GetButtonDown("Jump")) {
			if (this.jumps > 0) {
				this.jumps--;
				this.rigidbody2D.AddForce(new Vector2(0, this.rigidbody2D.velocity.x * -1000));
				this.rigidbody2D.AddForce(new Vector2(0, 500));
			}
			if (this.rigidbody2D.velocity.y == 0) {
				this.jumps = 1;
			}
		}

		if (this.getVelocity() > 0) {
			this.isMoving = true;
		} else {
			this.isMoving = false;
		}

		this.transform.Translate(this.movingVector); // apply speed change
	}

	float getVelocity() {
		return Mathf.Abs(this.rigidbody2D.velocity.x) + Mathf.Abs(this.rigidbody2D.velocity.y);
	}
}
