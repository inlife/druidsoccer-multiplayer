using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	const int SCORE_LIMIT = 9;

	public int leftScore  = 0;
	public int rightScore = 0;

	public bool canGoal = true;
	public bool finished = false;

	private GameObject team1;
	private GameObject team2;

	private void Start() {
		this.team1 = GameObject.Find("team1_win");
		this.team2 = GameObject.Find("team2_win");
		this.team1.SetActive(false);
		this.team2.SetActive(false);
	}
	
	public int onGoal(float position) {
		if (Network.isServer && this.canGoal && !this.finished) {
			if (position < 0) {
				if ( this.rightScore == SCORE_LIMIT ) {
					return this.onGameFinish(-1);
				} else 
					this.rightScore++;

				GameObject.Find("Player2").transform.position = new Vector3(6, 1.1f, 0);
			} else {
				if ( this.leftScore == SCORE_LIMIT ) {
					return this.onGameFinish(1);
				} else 
					this.leftScore++;

				GameObject.Find("Player1").transform.position = new Vector3(-6, 1.1f, 0);
			}
			networkView.RPC("updateText", RPCMode.All, new object[] { this.leftScore, this.rightScore, false });
			//GameObject.Find("squareball").transform.position = new Vector3(0, 2, 0);
			this.canGoal = false;
		}
		return 0;
	}

	private int onGameFinish(int side) {
		this.finished = true;
		networkView.RPC("showWinner", RPCMode.All, new object[] { side });
		networkView.RPC("updateText", RPCMode.All, new object[] { this.leftScore, this.rightScore, true });
		return side;
	}

	public void Update() {
		if (Network.isServer && this.finished) {
			if (Input.GetMouseButton(0)) {
				networkView.RPC("resetGame", RPCMode.All, null);
			}
		}
	}

	[RPC]
	public void showWinner(int side) {
		if (side > 0)
			this.team1.SetActive(true);
		else
			this.team2.SetActive(true);
	}

	[RPC]
	public void resetGame() {
		this.updateText(0, 0, false); // reset score

		this.team1.SetActive(false);
		this.team2.SetActive(false);

		GameObject.Find("squareball").transform.position = new Vector3(0, 2, 0); // reset ball
		
		GameObject.Find("Player1").transform.position = new Vector3(-6, 1.1f, 0);
		GameObject.Find("Player2").transform.position = new Vector3(6, 1.1f, 0);

		this.finished = false;
	}
	
	[RPC]
	public void updateText(int a, int b, bool showmsg) {
		this.leftScore  = a;
		this.rightScore = b;
		this.GetComponent<TextMesh>().text = a.ToString() + " : " + b.ToString();
		
		string msg = "";
		if (showmsg) {
			if (Network.isServer)
				msg = "Click to restart...";
			else if (Network.isClient)
				msg = "Waiting for the server...";
		}

		GameObject.Find("team_Message").GetComponent<TextMesh>().text = msg;
	}
}
