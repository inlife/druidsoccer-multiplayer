using UnityEngine;
using System.Collections;

public class Player2 : PlayerController {
	
	protected override void UpdateKeys() {

		if (Network.isClient) {
			if (Input.GetButtonDown("Jump")) {
				networkView.RPC("rpcJump", RPCMode.Server, null);
			}

			int input = 0;

			if (Input.GetKey(KeyCode.LeftArrow))
				input = -1;
			else if (Input.GetKey(KeyCode.RightArrow))
				input = 1;
			else
				input = 0;

			if (input != this.arrowkeyPrev ) {
				networkView.RPC("rpcMove", RPCMode.Server, new object[] { input });
				this.arrowkeyPrev = input;
			}
		}
	}
}
