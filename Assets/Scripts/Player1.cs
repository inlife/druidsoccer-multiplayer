using UnityEngine;
using System.Collections;

public class Player1 : PlayerController {

	protected override void UpdateKeys() {

		if ( Network.isServer ) {
			this.jumpkey = Input.GetButtonDown("Jump");

			if (Input.GetKey(KeyCode.LeftArrow))
				this.arrowkey = -1;
			else if (Input.GetKey(KeyCode.RightArrow))
				this.arrowkey = 1;
			else
				this.arrowkey = 0;
		}
	}
}
