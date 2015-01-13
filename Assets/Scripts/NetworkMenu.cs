using UnityEngine;
using System.Collections;

public class NetworkMenu : MonoBehaviour {

	public string ip = "176.8.247.124";
	public int port  = 27015;

	private Rect window = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300);

	private bool connected = false;

	private void OnConnectedToServer()
	{
		this.connected = true;
	}

	private void OnServerInitialized()
	{
		this.connected = true;
	}

	private void OnDisconnectedFromServer()
	{
		this.connected = false;
	}

	private void OnGUI() {

		if (!this.connected) {
			this.window = GUI.Window(0, this.window, this.windowGUI, "Connection");
		} else {
			GUILayout.Label("Connections: " + Network.connections.Length.ToString());
			if (Network.connections.Length > 0)
				GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]).ToString() + " ms");
		}
	}
	
	public void windowGUI(int windowID) {
		
		this.ip = GUILayout.TextField(this.ip);
		int.TryParse(GUILayout.TextField(this.port.ToString()), out this.port);
		
		if (GUILayout.Button("Connect")) {
			Network.Connect(this.ip, this.port);
		}
		
		if (GUILayout.Button("Host"))
			Network.InitializeServer(2, this.port, true);
	}
}
