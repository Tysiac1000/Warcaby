using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour {

	// elementy funkcjonalne
	private string winner;
	public int whoseTurnID;
	// elementy GUI
	// panele
	public GameObject panelGameEnd;
	public GameObject panelGrab;
	public Text textWinner;

	private GameObject player1,player2;

	// Use this for initialization
	// rozpoczęcie gry
	void Start () {
		createPlayer ("Player1", player1, 1);
		createPlayer ("Player2", player2, 2);

		whoseTurnID = 1;

		GameObject.Find("textStatus").GetComponent<Text>().text = "Player1";
		/*
		switch (GameObject.Find ("Options").GetComponent<Options> ().gMode) {
			case GameMode.ONE_DEVICE:
				createPlayer ("Player1", player1, 1);
				createPlayer ("Player2", player2, 2);
				break;
			case GameMode.HOST:
				break;
			case GameMode.CLIENT:
				break;
		};
		*/

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// tworzenie nowego gracza
	public void createPlayer(string playerName, GameObject player, int ID)
	{
		player = new GameObject ();
		player.AddComponent<Player> ();
		player.name = playerName;
		player.GetComponent<Player> ().playerID = ID;
	}
	// pokaż wynik gry
	public void showResults(string Winner){
		textWinner.text = Winner;
		panelGameEnd.SetActive (true);
	}
	// powrót do menu
	public void backToMainMenu(){
		// Sprawdzamy platformę i zamykamy program
		Application.LoadLevel ("menu");
	}
}
