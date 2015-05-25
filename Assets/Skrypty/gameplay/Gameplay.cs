using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Klasa Gameplay
/// </summary>
public class Gameplay : MonoBehaviour {
	/// <summary>
	/// elementy funkcjonalne
	/// </summary>
	private string winner;
	public int whoseTurnID;
	/// <summary>
	/// elementy GUI - Panele
	/// </summary>
	public GameObject panelGameEnd;
	public GameObject panelGrab;
	public Text textWinner;

	private GameObject player1,player2;

	// Use this for initialization
	/// <summary>
	/// rozpoczęcie gry
	/// </summary>
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
	/// <summary>
	/// tworzenie nowego gracza
	/// </summary>
	public void createPlayer(string playerName, GameObject player, int ID)
	{
		player = new GameObject ();
		player.AddComponent<Player> ();
		player.name = playerName;
		player.GetComponent<Player> ().playerID = ID;
	}
	/// <summary>
	/// pokaż wynik gry
	/// </summary>
	public void showResults(string Winner){
		textWinner.text = Winner;
		panelGameEnd.SetActive (true);
	}
	/// <summary>
	/// powrót do menu
	/// </summary>
	public void backToMainMenu(){
		/// <summary>
		/// Sprawdzamy platformę i zamykamy program
		/// </summary>
		Application.LoadLevel ("menu");
	}
}
