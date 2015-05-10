using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gameplay : MonoBehaviour {

	// elementy funkcjonalne
	private string winner;
	public int whoseTurnID;
	public bool isThereCapture;
	public bool isThereMove;

	// elementy GUI
	public GameObject panelGameEnd;
	public Text textWinner;
	public Text whoseTurnText;

	public GameObject Field_bright;
	public GameObject Field_dark;
	public Mesh boardMesh;
	public Material boardMaterial;

	// obiekty gry
	private GameObject Board;
	private List<GameObject> players;
	// komponenty 
	private Board BoardScript;

	// Use this for initialization
	void Start () {
		// tworzymy plansze
		createBoard ();

		// tworzymy graczy
		players = new List<GameObject>();
		createPlayer ("Player1", 1);
		createPlayer ("Player2", 2);

		// ustalamy kto rozpoczyna
		whoseTurnID = 1;
		isThereCapture = false;
		isThereMove = true;

		// ustawiamy komponent skryptu dla planszy
		BoardScript = Board.transform.GetComponent<Board> ();

		/*
		switch (GameObject.Find ("Options").GetComponent<Options> ().gMode) {
			case GameMode.ONE_DEVICE:
				createPlayer ("Player1", 1);
				createPlayer ("Player2", 2);
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

	// funkcja tworzy obiekt planszy
	private void createBoard() {
		// tworzymy obiekt planszy
		Board = new GameObject();
		Board.name = "Board";
		// dodajemy skrypt do planszy
		Board.AddComponent<Board>();
		Board BoardScript = Board.GetComponent<Board>();
		// przypisujemy materiały
		BoardScript.Field_bright = Field_bright;
		BoardScript.Field_dark = Field_dark;
		// dodajemy siatkę obiektu planszy
		Board.AddComponent<MeshFilter>().mesh = boardMesh;
		Board.GetComponent<MeshFilter> ().transform.localScale = new Vector3 ((float)3.782616,(float)3.782616,(float)3.782616);
		Board.GetComponent<MeshFilter> ().transform.Rotate(new Vector3 (270, 90, 0));
		Board.GetComponent<MeshFilter> ().transform.position = new Vector3 (0, (float)0.01, 0);
		Board.AddComponent<MeshRenderer> ().castShadows = false;
		Board.GetComponent<MeshRenderer> ().receiveShadows = true;
		// ustawiamy materiał dla siatki planszy
		Board.GetComponent<MeshRenderer> ().material = boardMaterial;
		Debug.Log ("Utworzono planszę");
	}

	// funckja tworzy gracza
	private void createPlayer(string playerName, int ID)
	{
		GameObject player = new GameObject ();
		player.AddComponent<Player> ();
		player.name = playerName;
		player.GetComponent<Player> ().playerID = ID;

		players.Add (player);

		Debug.Log ("Utworzono gracza");
	}

	// funkcja musi być wywoływana na początku każdej tury
	// aby sprawdzić czy są ruchy i bicia dla danego gracza
	public void onTurnStart(int pID){

		whoseTurnText.GetComponent<Text>().text = "Player " + pID;

		// ustawiamy wartość określającą czy jest jakieś bicie na fałsz
		isThereCapture = false;
		// ustawiamy wartość określającą czy jest jakiś ruch na fałsz
		isThereMove = false;
		
		// pobieramy listę pionków gracza
		List<GameObject> PlayerPawns = new List<GameObject>();
		for (int q = 0; q < players[pID-1].transform.childCount; q++) { 
			PlayerPawns.Add(players[pID-1].transform.GetChild(q).gameObject);
		}
		
		// trzeba sprawdzić każdy pionek pod względem możliwości wykonania ruchu bądź bicia
		for (int a = 0; a < PlayerPawns.Count; a++) {
			// pobieramy komponent skryptu dla aktualnie sprawdzanego pionka
			Pawn CurrentPawnScript = PlayerPawns[a].GetComponent<Pawn>();
			CurrentPawnScript.pState = PawnState.CANT_MOVE;

			// sprawdzamy czy pionek nie został ubity
			if (CurrentPawnScript.pState != PawnState.CAPTURED){

				// trzeba pobrać id pola na którym jest pionek
				string FieldId = CurrentPawnScript.fieldID;
				// tworzymy listę która będzie przechowywać id-ki sąsiadujących pól
				if (FieldId != "0") {
					List<string> closestFields = BoardScript.getSurroundingFields(FieldId);

					// w tej pętli sprawdzane będą wszystkie pola sąsiadujące, czy na którymś jest pionek wroga
					for  (int b = 0; b < closestFields.Count; b++) {

						// pobieramy komponent skryptu dla aktualnie sprawdzanego pola
						Field CurrentFieldScript = GameObject.Find (closestFields[b]).transform.GetComponent<Field>();

						// jeśli pole jest nie puste
						FieldState BorderFieldState = CurrentFieldScript.fState;
						if (BorderFieldState == FieldState.NON_EMPTY){
							int PawnIDonField = BoardScript.getPawnIDonField(closestFields[b]);
							int PlayerIDonField = (int)PawnIDonField / 100;

							// jeśli pionek wroga
							if (pID != PlayerIDonField) {
								// sprawdzamy czy w lini prostej za tym polem jest wolne pole
								string endFieldID = BoardScript.getFieldIDinLine(FieldId,closestFields[b]);

								if (endFieldID != "brak_pola"){
									// trzeba sprawdzić czy pole za jest wolne 
									// dlatego pobieramy status pola końcowego
									FieldState endFieldStatus = GameObject.Find (endFieldID).transform.GetComponent<Field>().fState;
									// sprawdzamy czy pole końcowe jest puste
									if (endFieldStatus != FieldState.NON_EMPTY){

										// jeśli pole za pionkiem wroga jest puste to mamy bicie
										GameObject.Find (endFieldID).transform.GetComponent<Field>().fState = FieldState.MOVE_ALLOWED;
										CurrentPawnScript.pState = PawnState.CAN_CAPTURE;
										//Debug.Log("Ustawiono dla pionka: "+CurrentPawnScript.name+", stan: "+CurrentPawnScript.pState);
										isThereCapture = true;

										Debug.Log("Bicie na: " + endFieldID);
									}
								}
							}
						} 
						// jeśli pole graniczne jest puste
						else if (BorderFieldState == FieldState.EMPTY || BorderFieldState == FieldState.MOVE_ALLOWED){
							// sprawdzamy czy pole graniczne jest z przodu
							if(BoardScript.isItFront(FieldId, closestFields[b], pID)) {
								// jeśli pole jest z przodu to ustawiamy MOVE_ALLOWED
								CurrentFieldScript.fState = FieldState.MOVE_ALLOWED;
								// a pionek moze wykonać ruch
								// jeśli pionek ma bicie to nie może wykonać ruchu
								if(CurrentPawnScript.pState != PawnState.CAN_CAPTURE) {
									CurrentPawnScript.pState = PawnState.CAN_MOVE;
									//Debug.Log("Ustawiono dla pionka: "+CurrentPawnScript.name+", stan: "+CurrentPawnScript.pState);
								}	

								isThereMove = true;
							}
						}
					}
				}

			}
		}

		// sprawdzamy czy są jakieś ruchy lub bicia
		if (!isThereCapture && !isThereMove) {
			// jeśli nie ma możliwych ruchów lub bić to koniec gry i remis
			showResults("Remis");
		}
		Debug.Log ("Zainicjalizowano turę dla gracza nr: " + whoseTurnID);
	}

	// zwraca gracza który posiada prawo ruchu
	public GameObject getEnemyPlayer(){
		if (whoseTurnID == 1) return GameObject.Find ("Player" + 2);
		else return GameObject.Find ("Player" + 1);
	}

	// funkcja zmieniająca gracza
	public void changeTurn(){
		
	}

	public void showResults(string Winner){
		if (Winner == "Remis") {
			// wyświetlenie remisu		
			panelGameEnd.SetActive (true);
		} else {
			// wyświetlenie informacji o tym który gracz wygrał
			textWinner.text = Winner;
			panelGameEnd.SetActive (true);
		}
	}

	public void backToMainMenu(){
		// Sprawdzamy platformę i zamykamy program
		Application.LoadLevel ("menu");
	}
}
