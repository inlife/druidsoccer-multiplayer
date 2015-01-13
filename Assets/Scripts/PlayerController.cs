using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	const float SPEED_LIMIT = 5;

	public bool isMoving = false;
	public bool inJump = false;

	protected int arrowkeyPrev = 0;
	protected int arrowkey = 0;
	protected bool jumpkey = false;

	private Animator animator;
	private Vector3 _data = Vector3.zero;

	protected virtual void UpdateKeys() {}

	protected void Start() {
		this.animator = this.GetComponent<Animator>();
	}

	protected void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "border_down") {
			this.inJump = false;
			this.jumpkey = false;
		} else if (collision.gameObject.name == "squareball") {
			this.inJump = false;
			this.jumpkey = false;
		}

	}

	protected void PhysicsUpdate()	{
		if (this.arrowkey > 0) {

			if (this.rigidbody2D.velocity.x < SPEED_LIMIT) {
				this.rigidbody2D.AddForce(new Vector2(800, 0));
			}
			this.animator.SetInteger("direction", 1);
			this.isMoving = true;

		} else if (this.arrowkey < 0) {

			if (this.rigidbody2D.velocity.x > -SPEED_LIMIT) {
				this.rigidbody2D.AddForce(new Vector2(-800, 0));
			}
			this.animator.SetInteger("direction", -1);
			this.isMoving = true;

		} else {
			this.isMoving = false;
		}

		if (this.jumpkey && !this.inJump) {
			this.rigidbody2D.AddForce(new Vector2(0, 13500));
			this.inJump = true;
		}

		this.animator.SetBool("run", this.isMoving);
	}
	
	void Update() {

		this.UpdateKeys();

		if (Network.isClient) 
			Destroy(this.rigidbody2D);
		
		if (Network.isServer && Network.connections.Length > 0) {
			this.PhysicsUpdate();
		}

		if (Network.isClient)
			this.UpdateMoving();
	}

	void UpdateMoving() {

		if (this._data.x == 6 && this._data.y == 1.1f) {
			this.transform.position = new Vector3(6, 1.1f, 0);
		} else if (this._data.x == -6 && this._data.y == 1.1f) {
			this.transform.position = new Vector3(-6, 1.1f, 0);
		} else if (this.transform.position.x != this._data.x || this.transform.position.y != this._data.y) {
			this.transform.position = Vector3.Slerp(transform.position, new Vector3(this._data.x, this._data.y, 0), 25 * Time.deltaTime);
		}
		
		if (Mathf.Abs(this.transform.position.x - this._data.x) > 0.05f) {
			this.animator.SetBool("run", true);
		} else {
			this.animator.SetBool("run", false);
		}

		this.animator.SetInteger("direction", (int)this._data.z);
	}
	
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) 
	{
		if (stream.isWriting) {
			
			this._data.x = this.transform.position.x;
			this._data.y = this.transform.position.y;
			this._data.z = this.animator.GetInteger("direction");
			
			stream.Serialize(ref this._data);
			
		} else {
			
			stream.Serialize(ref this._data);
			
		}
	}

	[RPC]
	public void rpcJump() {
		this.jumpkey = true;
	}

	[RPC]
	public void rpcMove(int side) {
		this.arrowkey = side;
	}
}
