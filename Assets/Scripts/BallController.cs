using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	private Vector3 _data = Vector3.zero;
	private Quaternion angleTo;

	void Start () {
	}

	bool ySpeedIsGoingTo() {
		return (Mathf.Abs(this.rigidbody2D.velocity.y) < 1 && this.transform.position.y < 3);
	}

	void Update () {
		if (Network.isClient) 
			Destroy(this.rigidbody2D);
		if (Network.isServer) {
			if (this.rigidbody2D.velocity.x > 4 && this.rigidbody2D.velocity.x < 6 && this.ySpeedIsGoingTo()) {
				this.rigidbody2D.AddTorque(13);
			}
			if (this.rigidbody2D.velocity.x < -4 && this.rigidbody2D.velocity.x > -6 && this.ySpeedIsGoingTo()) {
				this.rigidbody2D.AddTorque(-13);
			}
		}
		
		if (Network.isClient)
			this.UpdateMoving();
	}
	
	void UpdateMoving() {

		if (this._data.x == 0 && this._data.y == 2) {
			this.transform.position = new Vector3(0, 2, 0);
		} else if (this.transform.position.x != this._data.x || this.transform.position.y != this._data.y) {
			this.transform.position = Vector3.Slerp(transform.position, new Vector3(this._data.x, this._data.y, 0), 25 * Time.deltaTime);
		}

		if (this.transform.rotation.eulerAngles.z != this._data.z) {
			angleTo = Quaternion.AngleAxis(this._data.z, Vector3.forward);
			this.transform.rotation = Quaternion.Slerp(transform.rotation, angleTo, 25 * Time.deltaTime);
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) 
	{
		if (stream.isWriting) {

			this._data.x = this.transform.position.x;
			this._data.y = this.transform.position.y;
			this._data.z = this.transform.rotation.eulerAngles.z;

			stream.Serialize(ref this._data);

		} else {

			stream.Serialize(ref this._data);

		}
	}
}
